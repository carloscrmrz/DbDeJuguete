namespace DbDeJuguete.Models;

/// <summary>
/// Class to save into memory a database, used by the main application for working while not
/// persisting our DB into disk.
/// </summary>
/// <typeparam name="TSerializable">The type of objects to be used in the DB.</typeparam>
public class Database<TSerializable> where TSerializable : ISerializable
{
    /// <summary>
    /// A dictionary for fast access to our data, in this implementation we will
    /// use our object's Id for the <see cref="Dictionary{TKey,TValue}"/> key.
    /// </summary>
    public Dictionary<int, TSerializable> Data { get; init; }
    
    public Database()
    {
        Data = new Dictionary<int, TSerializable>();
    }
}