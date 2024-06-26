using Microsoft.AspNetCore.Identity.Data;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface IUsersRepository : IRepository<AppUser>
{
    public Task<AppUser?> GetUserByLogin(string login);
    public Task<AppUser?> GetUserByRefreshToken(string token);
}