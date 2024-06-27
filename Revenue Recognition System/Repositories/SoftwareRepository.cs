using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class SoftwareRepository : BaseRepository<Software>, ISoftwareRepository
{
    public SoftwareRepository(AppDbContext context) : base(context)
    {
    }
    
}