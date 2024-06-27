using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IExchangeService
{
    public Task<ExchangeModel?> GetExchangeRates(string currency);
}