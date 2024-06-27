using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface IDiscountRepository : IBaseRepository<Discount>
{
    public Task<ICollection<Discount>> GetDiscountsByIds(List<int> ids);
}