using DbDeJuguete.Models;

namespace DbDeJuguete.Services;

public interface ISerializerService
{
    /// <summary>
    /// Serializes the contents of a database into a CSV-compliant string so that it can be
    /// persisted into disk.
    /// </summary>
    /// <param name="database">The Database object that we will save to a CSV file.</param>
    /// <param name="filePath">The name of the database to save into the app's directory.</param>
    public bool Serialize(Database<ISerializable> database, string? filePath = "database");

    /// <summary>
    /// Deserializes the content of a CSV-compliant file and saves it into memory for usage
    /// within the app. 
    /// </summary>
    /// <param name="filePath">The name of the database in the app's directory.</param>
    /// <returns>Returns a Database object.</returns>
    public Database<ISerializable> Deserialize(string filePath = "database");

    /// <summary>
    /// Parses an object from the client's input.
    /// </summary>
    /// <returns>The object parsed from the STDIN</returns>
    public ISerializable ParseItem();

    /// <summary>
    /// Parses an object from updates made from the client's input. 
    /// </summary>
    /// <returns>The object parsed from the STDIN</returns>
    public ISerializable ParseUpdateItem(ISerializable currentItem);

    /// <summary>
    /// Imprime a pantalla la base de datos.
    /// </summary>
    /// <param name="db">La base de datos a imprimir.</param>
    public void PrintDatabase(Database<ISerializable> db);
}