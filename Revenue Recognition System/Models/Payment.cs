namespace Revenue_Recognition_System.Models;

public class Payment
{
    public int Id { get; set; }
    public int ContractId { get; set; }
    public SoftwareContract Contract { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsReturned { get; set; } = false;
}