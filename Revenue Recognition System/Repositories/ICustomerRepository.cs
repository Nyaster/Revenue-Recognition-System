using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    public Task DoSomeLogich();
}