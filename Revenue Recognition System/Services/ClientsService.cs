using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;

namespace Revenue_Recognition_System.Services;

public class ClientsService : IClientsService
{
    private IClientsRepository _clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }


    public async Task AddClient(IndividualDto client)
    {
        await CheckClientExist(client);

        await _clientsRepository.Add(new Individual()
        {
            Adress = client.Adress,
            Email = client.Email,
            FirstName = client.FirstName,
            IsDeleted = false,
            Pesel = client.Pesel,
            PhoneNumber = client.PhoneNumber,
            SecondName = client.SecondName,
        });
    }

    private async Task CheckClientExist(IndividualDto client)
    {
        if (await _clientsRepository.GetByPesel(client.Pesel) != null)
        {
            throw new UserArleadyExistException();
        }
    }


    public async Task Update(IndividualDto client, int id)
    {
        var byId = await _clientsRepository.GetById(id);
        CheckIfClientExist(byId);

        CheckPeselModification(client, byId);

        await _clientsRepository.Update(new Individual()
        {
            Id = id,
            Adress = client.Adress,
            Email = client.Email,
            FirstName = client.FirstName,
            SecondName = client.SecondName,
            IsDeleted = false,
            Pesel = client.Pesel,
            PhoneNumber = client.PhoneNumber,
        });
    }

    private void CheckPeselModification(IndividualDto client, AbstractClient? byId)
    {
        if (byId is Individual)
        {
            var individual = (Individual)byId;
            if (individual.Pesel != client.Pesel)
            {
                throw new PeselCanNotEditedExecption();
            }
        }
    }

    private void CheckIfClientExist(AbstractClient? byId)
    {
        if (byId == null || byId is Individual && ((Individual)byId).IsDeleted)
        {
            throw new ClientNotFoundException();
        }
    }

    public async Task Delete(int id)
    {
        var byId = await _clientsRepository.GetById(id);
        switch (byId)
        {
            case null:
                throw new UserNotFoundException(id.ToString());
                break;
            case Company company:
                throw new CannotDeleteCompantiesException();
                break;
            case Individual individual:
                individual.IsDeleted = true;
                individual.Pesel = "";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(byId));
        }

        await _clientsRepository.Update(byId);
    }

    public async Task<AbstractClientDTO> Get(int id)
    {
        var individual = await _clientsRepository.GetById(id);
        switch (individual)
        {
            case null:
                throw new UserNotFoundException(id.ToString());
                break;
            case Company company:
                return new CompanyDto()
                {
                    Adress = company.Adress,
                    CompanyName = company.CompanyName,
                    Email = company.Email,
                    Krs = company.Krs,
                    PhoneNumber = company.PhoneNumber,
                };
                break;
            case Individual individual1:
                CheckIfClientExist(individual1);
                return new IndividualDto()
                {
                    FirstName = individual1.FirstName,
                    SecondName = individual1.SecondName,
                    Adress = individual1.Adress,
                    PhoneNumber = individual1.PhoneNumber,
                    Pesel = individual1.Pesel,
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(individual));
        }
    }

    public async Task AddClient(CompanyDto client)
    {
        await _clientsRepository.Add(new Company()
        {
            Adress = client.Adress,
            Email = client.Email,
            CompanyName = client.CompanyName,
            Krs = client.Krs,
            PhoneNumber = client.PhoneNumber,
        });
    }

    public Task Update(CompanyDto client, int id)
    {
        throw new NotImplementedException();
    }
}

public class CannotDeleteCompantiesException : Exception
{
}

public class ClientNotFoundException : Exception
{
}

public class PeselCanNotEditedExecption : Exception
{
}