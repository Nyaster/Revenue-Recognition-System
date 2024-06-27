using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs;

public class ContractDto
{
    [Required] public int SoftwareId { get; set; }

    [MinLength(3)]
    [Required]
    [MaxLength(30)]
    public int TimeToPay { get; set; }

    [MinLength(1)]
    [Required]
    [MaxLength(3)]
    public int SupportYears { get; set; }

    public List<int>? Discounts { get; set; }
}