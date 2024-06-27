using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Repositories;
using System.Threading.Tasks;

namespace Revenue_Recognition_System.Services;

public class RevenueService : IRevenueService
{
    private readonly ISoftwareContractRepository _softwareContractRepository;
    private readonly IExchangeService _exchangeService;
    private readonly ISoftwareRepository _softwareRepository;
    private const string DefaultCurrency = "PLN";

    public RevenueService(ISoftwareContractRepository softwareContractRepository, IExchangeService exchangeService,
        ISoftwareRepository softwareRepository)
    {
        _softwareContractRepository = softwareContractRepository;
        _exchangeService = exchangeService;
        _softwareRepository = softwareRepository;
    }

    public async Task<RevenueResponceModel> CalculateFutureRevenue(RevenueRequestModel revenueRequestModel)
    {
        return await CalculateRevenueInternal(revenueRequestModel, true);
    }

    public async Task<RevenueResponceModel> CalculateRevenue(RevenueRequestModel revenueRequestModel)
    {
        return await CalculateRevenueInternal(revenueRequestModel, false);
    }

    private async Task<RevenueResponceModel> CalculateRevenueInternal(RevenueRequestModel revenueRequestModel,
        bool isFuture)
    {
        decimal exchangeRate = 1;
        if (revenueRequestModel.ConvertTo != null)
        {
            var exchangeRates = await _exchangeService.GetExchangeRates(DefaultCurrency);
            if (exchangeRates != null && exchangeRates.Rates.ContainsKey(revenueRequestModel.ConvertTo.ToUpper()))
            {
                exchangeRate = exchangeRates.Rates[revenueRequestModel.ConvertTo.ToUpper()];
            }
            else
            {
                revenueRequestModel.ConvertTo = DefaultCurrency;
            }
        }

        var wholeCompany = "Whole Company";
        string forWhat = (revenueRequestModel.SoftwareId == null
            ? wholeCompany
            : revenueRequestModel.SoftwareId.ToString())!;
        if (revenueRequestModel.SoftwareId != null)
        {
            var softwareContract = await _softwareRepository.GetById((int)revenueRequestModel.SoftwareId);
            if (softwareContract == null)
            {
                throw new ClientNotFoundException();
            }
        }

        var revenue = forWhat == wholeCompany
            ? isFuture
                ? await _softwareContractRepository.GetAllMoney()
                : await _softwareContractRepository.GetPayedAllMoney()
            : isFuture
                ? await _softwareContractRepository.GetAllMoney(revenueRequestModel.SoftwareId)
                : await _softwareContractRepository.GetPayedAllMoney(revenueRequestModel.SoftwareId);

        var revenueResponceModel = new RevenueResponceModel()
        {
            ForWhat = forWhat,
            Amount = revenue * exchangeRate,
            Currency = revenueRequestModel.ConvertTo ?? DefaultCurrency,
        };

        return revenueResponceModel;
    }
}