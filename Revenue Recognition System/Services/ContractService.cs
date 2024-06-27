using Microsoft.IdentityModel.Tokens;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;

namespace Revenue_Recognition_System.Services;

public class ContractService : IContractsService
{
    private readonly IClientsRepository _clientsRepository;
    private ISoftwareRepository _softwareRepository;
    private IDiscountRepository _discountRepository;
    private ISoftwareContractRepository _softwareContractRepository;
    private IPaymentRepository _paymentRepository;

    public ContractService(IClientsRepository clientsRepository, IDiscountRepository discountRepository,
        ISoftwareRepository softwareRepository, ISoftwareContractRepository softwareContractRepository,
        IPaymentRepository paymentRepository)
    {
        _clientsRepository = clientsRepository;
        _discountRepository = discountRepository;
        _softwareRepository = softwareRepository;
        _softwareContractRepository = softwareContractRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task CreateContract(ContractDto contractDto, int id)
    {
        int discountPercent = 0;
        var clientById = await _clientsRepository.GetClientById(id);
        if (clientById == null || clientById is Individual && ((Individual)clientById).IsDeleted)
        {
            throw new UserNotFoundException(id.ToString());
        }

        var sowtware = await _softwareRepository.GetById(contractDto.SoftwareId);
        if (sowtware == null)
        {
            throw new UserNotFoundException(id.ToString());
        }

        var byId = await _softwareRepository.GetById(contractDto.SoftwareId);
        if (byId == null)
        {
            throw new ClientNotFoundException();
        }

        if (clientById.SoftWareContracts != null && clientById.SoftWareContracts.Any(x => x.IsPaid == true))
        {
            discountPercent += 5;
        }

        ICollection<Discount>? discounts = null;
        if (contractDto.Discounts != null)
        {
            discounts = await _discountRepository.GetDiscountsByIds(contractDto.Discounts);
        }

        var percentage = discounts.MaxBy(x => x.Percentage);
        discountPercent += percentage?.Percentage ?? 0;

        const int pricePerYearSupport = 1000;
        var softWareContract = new SoftwareContract()
        {
            Client = clientById,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(contractDto.TimeToPay),
            IsPaid = false,
            IsActive = true,
            SupportYears = contractDto.SupportYears,
            Software = sowtware,
            Price = sowtware.Price + (contractDto.SupportYears * pricePerYearSupport) -
                    ((contractDto.SupportYears * pricePerYearSupport + sowtware.Price) *
                     (decimal)(discountPercent / 100.0)),
        };
        await _softwareContractRepository.Add(softWareContract);
    }

    public async Task<PaymentDto> ProcessPayment(int id, decimal amount)
    {
        var byId = await _softwareContractRepository.GetById(id);
        if (byId == null)
        {
            throw new ClientNotFoundException();
        }

        var priceToPayForSoftware = await _paymentRepository.GetArleadyPayedForSoftware(id);
        priceToPayForSoftware = byId.Price - priceToPayForSoftware;
        if (priceToPayForSoftware <= 0)
        {
            byId.IsPaid = true;
            await _softwareContractRepository.Update(byId);
        }

        if (byId.IsPaid)
        {
            throw new ClientArleadyPayedException();
        }

        if (!byId.IsActive || byId.EndDate < DateTime.Now)
        {
            throw new ContractTimeToPayExceesException();
        }

        if (priceToPayForSoftware < amount)
        {
            amount = priceToPayForSoftware;
        }

        var payment = new Payment()
        {
            Amount = amount,
            ContractId = id,
            IsReturned = false,
            PaymentDate = DateTime.Now,
        };

        await _paymentRepository.Add(payment);
        byId.IsPaid = (priceToPayForSoftware - amount) <= 0;
        await _softwareContractRepository.Update(byId);
        var paymentDto = new PaymentDto()
        {
            DueDate = byId.EndDate,
            Payed = amount,
            ToBePayed = priceToPayForSoftware - amount
        };
        return paymentDto;
    }
}

public class ContractTimeToPayExceesException : Exception
{
}

public class ClientArleadyPayedException : Exception
{
}