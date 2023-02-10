using System.Text;
using ConsoleTables;
using DbDeJuguete.Helpers;
using DbDeJuguete.Models;

namespace DbDeJuguete.Services;

public class ProductSerializerService : ISerializerService
{
    /// <summary>
    /// A buffer where the service will build the CSV file. As the Service will be used as a
    /// Singleton we need to take care of disposing the contents after flushing to the file.
    /// </summary>
    private readonly StringBuilder _buffer = new();

    private static readonly string BasePath = Path.Combine(Directory.GetCurrentDirectory(), "database", "products");

    /// <summary>
    /// Serializes the object into a CSV row.
    /// </summary>
    /// <param name="value">The object to be serialized.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> with the object's content in a CSV row form.</returns>
    private static IEnumerable<string> SerializeMapper(ISerializable value)
    {
        return value.GetPropertiesValues();
    }

    private Product DeseriliazeMapper(string value)
    {
        var propertiesList = value.Split(',').ToList();

        var productId = 0;
        var productPrice = 0M;
        var productQuantity = 0;
        try
        {
            productId = int.Parse(propertiesList[0]);
            productPrice = decimal.Parse(propertiesList[3]);
            productQuantity = int.Parse(propertiesList[4]);
        }
        catch (Exception)
        {
            // Ignored
        }

        return new Product
        {
            Id = productId,
            Name = propertiesList[1],
            Category = Enum.Parse<Category>(propertiesList[2], true),
            Price = productPrice,
            Quantity = productQuantity
        };
    }

    public bool Serialize(Database<ISerializable> database, string? filePath = "database")
    {
        try
        {
            var fields = SerializerServiceHelpers.GetObjectFields<Product>();
            _buffer.AppendJoin(',', fields);
            _buffer.AppendLine();
            foreach (var value in database.Data.Values)
            {
                _buffer.AppendJoin(',', SerializeMapper(value));
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

        Console.WriteLine("Nombre del producto: ");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);
        Console.WriteLine(
            "Categoria del producto: (0 - Antibiotico, 1- Antiinflamatorio, 2 - Analgesico, 3 - Comida, 4 - Bebidas, 5 - Otros)");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Precio del producto: ");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Stock del producto: ");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);

        var productPrice = 0M;
        var productQuantity = 0;
        Category category = 0;
        try
        {
            category = Enum.Parse<Category>(propertiesList[1], true);
            productPrice = decimal.Parse(propertiesList[2]);
            productQuantity = int.Parse(propertiesList[3]);
        }
        catch (Exception)
        {
            // Ignored
        }

        return new Product
        {
            Id = 0,
            Name = propertiesList[0],
            Category = category,
            Price = productPrice,
            Quantity = productQuantity
        };
    }

    public ISerializable ParseUpdateItem(ISerializable currentItem)
    {
        var propertiesList = new List<string>();
        var product = currentItem as Product;

        Console.WriteLine($"Nombre del producto: {product?.Name}");
        Console.WriteLine("Nuevo nombre del producto: (deje en blanco para no actualizar)");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);
        Console.WriteLine($"Categoria del producto: {product?.Category}");
        Console.WriteLine("Nueva categoria del producto: (deje en blanco para no actualizar)");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);
        Console.WriteLine($"Precio del producto: {product?.Price}");
        Console.WriteLine("Nuevo precio del producto: (deje en blanco para no actualizar)");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);
        Console.WriteLine($"Stock del producto: {product?.Quantity}");
        Console.WriteLine("Nuevo stock del producto: (deje en blanco para no actualizar)");
        propertiesList.Add(Console.ReadLine() ?? string.Empty);

        var productPrice = 0M;
        var productQuantity = 0;
        Category category = 0;
        try
        {
            category = propertiesList[1] is "" ? product!.Category : Enum.Parse<Category>(propertiesList[1], true);
            productPrice = propertiesList[2] is "" ? product!.Price : decimal.Parse(propertiesList[2]);
            productQuantity = propertiesList[3] is "" ? product!.Quantity : int.Parse(propertiesList[3]);
        }
        catch (Exception)
        {
            // Ignored
        }

        return new Product
        {
            Id = currentItem.Id,
            Name = propertiesList[0] is "" ? product!.Name : propertiesList[0],
            Category = category,
            Price = productPrice,
            Quantity = productQuantity
        };
    }

    public void PrintDatabase(Database<ISerializable> db)
    {
        try
        {
            var dbEnumerable = db.Data.Values.Select<ISerializable, Product>(p => (Product)p);
            ConsoleTable.From(dbEnumerable).Write();
        }
        catch (Exception)
        {
            ConsoleTable.From(Array.Empty<Product>()).Write();
        }
    }
}