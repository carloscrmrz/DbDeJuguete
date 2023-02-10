using System.Text;
using ConsoleTables;
using DbDeJuguete.Models;

namespace DbDeJuguete.Services;

public class RevenueSerializerService : ISerializerService
{
    /// <summary>
    /// A buffer where the service will build the CSV file. As the Service will be used as a
    /// Singleton we need to take care of disposing the contents after flushing to the file.
    /// </summary>
    private readonly StringBuilder _buffer = new();

    private static readonly string BasePath = Path.Combine(Directory.GetCurrentDirectory(), "database", "revenue");

    public bool Serialize(Database<ISerializable> database, string? filePath = "database")
    {
        try
        {
            var fields = new List<string> { "Id", "Total", "Day", "Month", "Year" };
            _buffer.AppendJoin(',', fields);
            _buffer.AppendLine();
            foreach (var value in database.Data.Values.Cast<Revenue?>())
            {
                _buffer.Append(
                    $"{value?.Id},{value?.Total},{value?.Date.Day},{value?.Date.Month},{value?.Date.Year}");
                _buffer.AppendLine();
            }

            Directory.CreateDirectory(BasePath);
            var dbPath = Path.Combine(BasePath, $"{filePath}.db");

            File.WriteAllText($"{dbPath}", _buffer.ToString());
            _buffer.Clear();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Database<ISerializable> Deserialize(string filePath = "database")
    {
        var dictionary = new Dictionary<int, ISerializable>();

        Directory.CreateDirectory(BasePath);
        var dbPath = Path.Combine(BasePath, $"{filePath}.db");

        using var sr = new StreamReader($"{dbPath}");
        sr.ReadLine(); // First line with field names.

        while (sr.ReadLine() is { } line)
        {
            var value = DeseriliazeMapper(line);
            dictionary.Add(value.Id, value);
        }

        return new Database<ISerializable> { Data = dictionary };
    }

    public ISerializable ParseItem()
    {
        var propertiesList = new List<string>();

        Console.WriteLine("Total del dia: ");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);

        var revenueDate = DateTime.Today;
        var revenueTotal = 0M;
        try
        {
            revenueTotal = int.Parse(propertiesList[0]);
        }
        catch (Exception)
        {
            // ignored
        }

        return new Revenue
        {
            Id = 0,
            Date = revenueDate,
            Total = revenueTotal
        };
    }

    public ISerializable ParseUpdateItem(ISerializable currentItem)
    {
        var propertiesList = new List<string>();
        var revenue = currentItem as Revenue;
        
        Console.WriteLine($"Total del dia: {revenue?.Total}");
        Console.WriteLine("Nuevo total del dia: (deje en blanco para no actualizar)");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);

        var revenueDate = DateTime.Today;
        var revenueTotal = revenue?.Total;
        try
        {
            revenueTotal = propertiesList[0] is "" ? revenue?.Total : int.Parse(propertiesList[0]);
        }
        catch (Exception)
        {
            // ignored
        }

        return new Revenue
        {
            Id = currentItem.Id,
            Date = revenueDate,
            Total = revenueTotal.GetValueOrDefault(0M)
        };
    }

    private Revenue DeseriliazeMapper(string value)
    {
        var propertiesList = value.Split(',').ToList();

        var revenueId = 0;
        DateTime revenueDate;
        var revenueTotal = 0M;
        try
        {
            revenueId = int.Parse(propertiesList[0]);
            revenueTotal = int.Parse(propertiesList[1]);
            revenueDate = new DateTime(
                int.Parse(propertiesList[4]),
                int.Parse(propertiesList[3]),
                int.Parse(propertiesList[2]));
        }
        catch (Exception)
        {
            revenueDate = DateTime.Today; // Si no se puede procesar la fecha se hace fallback al dia de hoy.
        }

        return new Revenue
        {
            Id = revenueId,
            Date = revenueDate,
            Total = revenueTotal
        };
    }

    public void PrintDatabase(Database<ISerializable> db)
    {
        try
        {
            var dbEnumerable = db.Data.Values.Select<ISerializable, Revenue>(p => (Revenue)p);
            ConsoleTable.From(dbEnumerable).Write();
        }
        catch (Exception)
        {
            ConsoleTable.From(Array.Empty<Revenue>()).Write();
        }
    }
}