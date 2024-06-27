using System.Text.Json;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public class ExchangeRateService : IExchangeService
{
    private readonly HttpClient _httpClient;
    private const string Api = "https://v6.exchangerate-api.com/v6";
    private readonly string _apiKey;

    public ExchangeRateService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ApiKey"];
    }

    public async Task<ExchangeModel?> GetExchangeRates(string currency)
    {
        try
        {
            var url = $"{Api}/{_apiKey}/latest/{currency}";
            var responce = await _httpClient.GetFromJsonAsync<ExchangeModel>(url);
            ;
            if (responce == null)
            {
                throw new Exception("No data from api");
            }

            return responce;
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while fetching exchange rates.", e);
        }
    }
}