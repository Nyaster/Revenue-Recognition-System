namespace Revenue_Recognition_System.DTOs;

public class PaymentDto
{
    public decimal Payed { get; set; }
    public decimal ToBePayed { get; set; }
    public DateTime DueDate { get; set; }
}