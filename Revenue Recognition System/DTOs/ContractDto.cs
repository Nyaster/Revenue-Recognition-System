using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs;

public class ContractDto
{
    [Required] public int SoftwareId { get; set; }
    [Required]
    [Range(3,30)]
    public int TimeToPay { get; set; }

    [Range(1,3)]
    [Required]
    public int SupportYears { get; set; }

    public ICollection<int>? Discounts { get; set; }
}