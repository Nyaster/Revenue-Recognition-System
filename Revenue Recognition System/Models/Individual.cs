namespace Revenue_Recognition_System.Models;

public class Individual : AbstractClient
{

    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Pesel { get; set; }
    public bool IsDeleted { get; set; }
}