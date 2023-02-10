namespace DbDeJuguete.Models;

public class Product : ISerializable
{
    public int Id { get; set; }
    public string? Name { get; init; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public enum Category
{
    Antibiotic,
    Antiinflamatory,
    Analgesic,
    Food,
    Beverage,
    Others,
}