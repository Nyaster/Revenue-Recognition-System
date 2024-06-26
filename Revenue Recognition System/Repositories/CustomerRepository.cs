using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public Task DoSomeLogich()
    {
        throw new NotImplementedException();
    }
}