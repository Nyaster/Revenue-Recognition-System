using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IContractsService
{
    public Task CreateContract(ContractDto contractDto, int id);
    public Task<PaymentDto> ProcessPayment(int id, decimal amount);
    public Task<ICollection<SoftwareContract>> GetAll();
}