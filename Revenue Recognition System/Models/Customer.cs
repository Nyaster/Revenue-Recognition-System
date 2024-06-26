namespace Revenue_Recognition_System.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Adress { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Pesel { get; set; }
    public bool IsDeleted { get; set; }
}