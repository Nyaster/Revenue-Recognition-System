using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.DTOs;

public class IndividualDto : AbstractClientDTO
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Pesel { get; set; }
}