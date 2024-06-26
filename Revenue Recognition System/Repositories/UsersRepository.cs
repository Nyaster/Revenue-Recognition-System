using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class UsersRepository : BaseRepository<AppUser>, IUsersRepository
{
    public UsersRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<AppUser?> GetUserByLogin(string login)
    {
        return await _dbSet.Where(x => x.Login == login).FirstOrDefaultAsync();
    }

    public async Task<AppUser?> GetUserByRefreshToken(string token)
    {
        return await _dbSet.Where(x => x.RefreshToken == token).FirstOrDefaultAsync();
    }
}