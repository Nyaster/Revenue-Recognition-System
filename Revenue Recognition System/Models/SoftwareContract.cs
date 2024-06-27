namespace Revenue_Recognition_System.Models;

public class SoftwareContract
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public AbstractClient Client { get; set; }
    public Software Software { get; set; }
    public int SoftwareId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int SupportYears { get; set; }
}