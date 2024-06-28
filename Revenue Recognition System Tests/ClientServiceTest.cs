using Moq;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System_Tests;

public class ClientServiceTest
{
    private readonly Mock<IClientsRepository> _clientsRepositoryMock;
    private readonly ClientsService _clientsService;

    public ClientServiceTest()
    {
        _clientsRepositoryMock = new Mock<IClientsRepository>();
        _clientsService = new ClientsService(_clientsRepositoryMock.Object);
    }

    [Fact]
    public async Task AddClientIndividual_Should_Add_New_Client()
    {
        // Arrange
        var clientDto = new IndividualDto
        {
            Adress = "osowsk wasaw",
            Email = "email@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "1234567890",
            SecondName = "smith"
        };

        _clientsRepositoryMock.Setup(repo => repo.GetByPesel(It.IsAny<string>())).ReturnsAsync((Individual)null);

        // Act
        await _clientsService.AddClient(clientDto);

        // Assert
        _clientsRepositoryMock.Verify(repo => repo.Add(It.IsAny<Individual>()), Times.Once);
    }
    [Fact]
    public async Task AddClientCompanyl_Should_Add_New_Client()
    {
        // Arrange
        var clientDto = new CompanyDto()
        {
            Adress = "osowsk wasaw",
            Email = "email@example.com",
            CompanyName = "John",
            Krs = "1234567890",
            PhoneNumber = "1234567890",
        };

        _clientsRepositoryMock.Setup(repo => repo.GetClientByKrs(It.IsAny<string>())).ReturnsAsync((Company)null);

        // Act
        await _clientsService.AddClient(clientDto);

        // Assert
        _clientsRepositoryMock.Verify(repo => repo.Add(It.IsAny<Company>()), Times.Once);
    }

    [Fact]
    public async Task AddClient_Should_Throw_Exception_When_Client_Exists()
    {
        // Arrange
        var clientDto = new IndividualDto
        {
            Adress = "123 Main St",
            Email = "email@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "1234567890",
            SecondName = "Doe"
        };

        _clientsRepositoryMock.Setup(repo => repo.GetByPesel(It.IsAny<string>())).ReturnsAsync(new Individual());

        // Act & Assert
        await Assert.ThrowsAsync<UserArleadyExistException>(() => _clientsService.AddClient(clientDto));
    }

    [Fact]
    public async Task UpdateClient_Should_Update_Client_Details()
    {
        // Arrange
        var clientDto = new IndividualDto
        {
            Adress = "123 Main St",
            Email = "newemail@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "0987654321",
            SecondName = "Doe"
        };

        var existingClient = new Individual
        {
            Id = 1,
            Adress = "123 Old St",
            Email = "oldemail@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "1234567890",
            SecondName = "Doe",
            IsDeleted = false
        };

        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingClient);

        // Act
        await _clientsService.Update(clientDto, 1);

        // Assert
        _clientsRepositoryMock.Verify(
            repo => repo.Update(It.Is<Individual>(c =>
                c.Email == clientDto.Email && c.PhoneNumber == clientDto.PhoneNumber)), Times.Once);
    }
    [Fact]
    public async Task UpdateClientCompany_Should_Update_Client_Details()
    {
        // Arrange
        var clientDto = new CompanyDto()
        {
            Adress = "osowsk wasaw",
            Email = "email@example.com",
            CompanyName = "John",
            Krs = "1234567890",
            PhoneNumber = "1234567891",
        };

        var existingClient = new Company()
        {
            Id = 1,
            Adress = "123 Old St",
            Email = "oldemail@example.com",
            Krs = "1234567890",
            PhoneNumber = "1234567890",
            CompanyName = "Enigma"
        };

        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingClient);

        // Act
        await _clientsService.Update(clientDto, 1);

        // Assert
        _clientsRepositoryMock.Verify(
            repo => repo.Update(It.Is<Company>(c =>
                c.Email == clientDto.Email && c.PhoneNumber == clientDto.PhoneNumber)), Times.Once);
    }

    [Fact]
    public async Task UpdateClient_Should_Throw_Exception_When_Client_Not_Found()
    {
        // Arrange
        var clientDto = new IndividualDto
        {
            Adress = "123 Main St",
            Email = "newemail@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "0987654321",
            SecondName = "Doe"
        };
        // Act 
        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((AbstractClient)null);

        // Assert
        await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientsService.Update(clientDto, 1));
    }

    [Fact]
    public async Task DeleteClient_Should_Set_IsDeleted_To_True()
    {
        // Arrange
        var existingClient = new Individual
        {
            Id = 1,
            IsDeleted = false,
            Pesel = "1234567890"
        };

        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingClient);

        // Act
        await _clientsService.Delete(1);

        // Assert
        Assert.True(existingClient.IsDeleted);
        Assert.Equal("", existingClient.Pesel);
        _clientsRepositoryMock.Verify(repo => repo.Update(It.Is<Individual>(c => c.IsDeleted == true && c.Pesel == "")),
            Times.Once);
    }

    [Fact]
    public async Task DeleteClient_Should_Throw_Exception_When_Client_Not_Found()
    {
        // Arrange

        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((AbstractClient)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _clientsService.Delete(1));
    }

    [Fact]
    public async Task Update_Should_Throw_Exception_When_Trying_Change_Pesel()
    {
        // Arrange
        var clientDto = new IndividualDto
        {
            Adress = "123 Main St",
            Email = "newemail@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "0987654321",
            SecondName = "Doe"
        };
        var existingClient = new Individual
        {
            Id = 1,
            Adress = "123 Old St",
            Email = "oldemail@example.com",
            FirstName = "John",
            Pesel = "12345678901",
            PhoneNumber = "1234567890",
            SecondName = "Doe",
            IsDeleted = false
        };
        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingClient);
        await Assert.ThrowsAsync<PeselCanNotEditedExecption>(() => _clientsService.Update(clientDto, 1));
    }

    [Fact]
    public async Task GetClient_Should_Return_Client_Dto()
    {
        // Arrange
        var existingClient = new Individual
        {
            Id = 1,
            Adress = "123 Main St",
            Email = "email@example.com",
            FirstName = "John",
            Pesel = "1234567890",
            PhoneNumber = "1234567890",
            SecondName = "Doe",
            IsDeleted = false
        };

        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingClient);

        // Act
        var result = await _clientsService.Get(1);

        // Assert
        Assert.IsType<IndividualDto>(result);
        Assert.Equal("John", ((IndividualDto)result).FirstName);
    }

    [Fact]
    async Task GetClient_SHould_Return_Company_Dto()
    {
        // Arrange
        var existingClient = new Company()
        {
            Id = 1,
            Adress = "123 Main St",
            Email = "email@example.com",
            PhoneNumber = "1234567890",
            CompanyName = "erbcorp",
            Krs = "1234567890"
        };
        // Act
        _clientsRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingClient);
        var result = await _clientsService.Get(1);

        // Assert
        Assert.IsType<CompanyDto>(result);
        Assert.Equal("1234567890", ((CompanyDto)result).Krs);
    }
}