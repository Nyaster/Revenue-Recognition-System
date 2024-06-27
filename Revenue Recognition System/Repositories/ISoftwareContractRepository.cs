using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface ISoftwareContractRepository : IBaseRepository<SoftwareContract>
{
    public Task<decimal> GetPayedAllMoney();
    public Task<decimal> GetPayedAllMoney(int? softwareId);
    public Task<decimal> GetAllMoney();
    public Task<decimal> GetAllMoney(int? softwareId);
}