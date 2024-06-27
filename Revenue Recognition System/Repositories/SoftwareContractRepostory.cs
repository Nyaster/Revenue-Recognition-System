using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class SoftwareContractRepostory : BaseRepository<SoftwareContract>, ISoftwareContractRepository
{
    public SoftwareContractRepostory(AppDbContext context) : base(context)
    {
    }
}