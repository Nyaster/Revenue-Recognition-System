using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    public Task<decimal> GetPriceToPayForSoftware(int softwareContractId);
}