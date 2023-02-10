using DbDeJuguete.Models;

namespace DbDeJuguete.Services;

public class DatabaseService
{
    private readonly ISerializerService _serializerService;
    private Database<ISerializable> Database { get; set; } = null!;

    /// <summary>
    /// Initializes our database application, 
    /// </summary>
    public DatabaseService(ISerializerService serializerService)
    {
        _serializerService = serializerService;
    }

    public bool Create()
    {
        var item = _serializerService.ParseItem();
        var maxId = 1;
        try
        {
            maxId = Database.Data.Keys.Max() + 1;
        }
        catch (Exception)
        {
            // ignored
        }

        item.Id = maxId;

        return Database.Data.TryAdd(maxId, item);
    }

    public bool Update(int itemId)
    {
        try
        {
            var item = _serializerService.ParseUpdateItem(Database.Data[itemId]);
            Database.Data[itemId] = item;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Delete(int itemId)
    {
        return Database.Data.Remove(itemId);
    }


    public bool SaveDatabase(string dbName)
    {
        return _serializerService.Serialize(Database, dbName);
    }

    public void LoadDatabase(string filePath)
    {
        Database = _serializerService.Deserialize(filePath);
    }

    public void CreateDatabase()
    {
        Database = new Database<ISerializable>();
    }

    public void PrintDatabase()
    {
        _serializerService.PrintDatabase(Database);
    }
}