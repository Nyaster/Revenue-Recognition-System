namespace Revenue_Recognition_System.Models;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public DateTime ReleaseDate { get; set; }
}