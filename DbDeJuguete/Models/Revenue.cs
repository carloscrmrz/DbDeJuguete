namespace DbDeJuguete.Models;

public class Revenue : ISerializable
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
}