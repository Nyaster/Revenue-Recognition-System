using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;

namespace Revenue_Recognition_System.Services;

public class IndividualService : IIndividualService
{
    private IIndividualRepository _repository;

    public IndividualService(IIndividualRepository repository)
    {
        _repository = repository;
    }

    public async Task AddClient(IndividualDto client)
    {
        await CheckClientExist(client);

        await _repository.Add(new Individual()
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
        if (await _repository.GetByPesel(client.Pesel) != null)
        {
            throw new UserArleadyExistException();
        }
    }


    public async Task Update(IndividualDto client, int id)
    {
        var byId = await _repository.GetById(id);
        CheckIfClientExist(byId);

        CheckPeselModification(client, byId);

        await _repository.Update(new Individual()
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

    private void CheckPeselModification(IndividualDto client, Individual? byId)
    {
        if (byId.Pesel != client.Pesel)
        {
            throw new PeselCanNotEditedExecption();
        }
    }

    private void CheckIfClientExist(Individual? byId)
    {
        if (byId == null || byId.IsDeleted)
        {
            throw new ClientNotFoundException();
        }
    }

    public async Task Delete(int id)
    {
        var byId = await _repository.GetById(id);
        byId.IsDeleted = true;
        byId.Pesel = "";
        await _repository.Update(byId);
    }

    public async Task<IndividualDto> Get(int id)
    {
        var individual = await _repository.GetById(id);
        CheckIfClientExist(individual);
        return new IndividualDto()
        {
            FirstName = individual.FirstName,
            SecondName = individual.SecondName,
            Adress = individual.Adress,
            PhoneNumber = individual.PhoneNumber,
            Pesel = individual.Pesel,
        };
    }
}

public class ClientNotFoundException : Exception
{
}

public class PeselCanNotEditedExecption : Exception
{
}