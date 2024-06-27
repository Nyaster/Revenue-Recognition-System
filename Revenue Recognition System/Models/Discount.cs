namespace Revenue_Recognition_System.Models;

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool AppliesToSubscription { get; set; }
}