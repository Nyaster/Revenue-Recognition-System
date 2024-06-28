using Moq;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Tests
{
    public class ContractServiceTest
    {
        private readonly Mock<IClientsRepository> _clientsRepositoryMock;
        private readonly Mock<IDiscountRepository> _discountRepositoryMock;
        private readonly Mock<ISoftwareRepository> _softwareRepositoryMock;
        private readonly Mock<ISoftwareContractRepository> _softwareContractRepositoryMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly ContractService _contractService;

        public ContractServiceTest()
        {
            _clientsRepositoryMock = new Mock<IClientsRepository>();
            _discountRepositoryMock = new Mock<IDiscountRepository>();
            _softwareRepositoryMock = new Mock<ISoftwareRepository>();
            _softwareContractRepositoryMock = new Mock<ISoftwareContractRepository>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();

            _contractService = new ContractService(
                _clientsRepositoryMock.Object,
                _discountRepositoryMock.Object,
                _softwareRepositoryMock.Object,
                _softwareContractRepositoryMock.Object,
                _paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateContract_ShouldCreateContractSuccessfully()
        {
            // Arrange
            var contractDto = new ContractDto
            {
                SoftwareId = 1,
                SupportYears = 2,
                TimeToPay = 10,
                Discounts = new List<int> { 1 }
            };
            var client = new Individual { Id = 1, IsDeleted = false, SoftWareContracts = new List<SoftwareContract>() };
            var software = new Software { Id = 1, Price = 1000 };
            var discounts = new List<Discount> { new Discount { Id = 1, Percentage = 10 } };

            _clientsRepositoryMock.Setup(r => r.GetClientById(It.IsAny<int>())).ReturnsAsync(client);
            _softwareRepositoryMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync(software);
            _discountRepositoryMock.Setup(r => r.GetDiscountsByIds(It.IsAny<List<int>>())).ReturnsAsync(discounts);
            _softwareContractRepositoryMock.Setup(r => r.Add(It.IsAny<SoftwareContract>())).Returns(Task.CompletedTask);

            // Act
            await _contractService.CreateContract(contractDto, 1);

            // Assert
            _softwareContractRepositoryMock.Verify(r => r.Add(It.Is<SoftwareContract>(sc =>
                    sc.Client == client &&
                    sc.Software == software &&
                    sc.Price == 3700 // 1000 * 2 + 2 * 1000 - (2000 + 1000) * 0.10
            )), Times.Once);
        }

        [Fact]
        public async Task CreateContract_ShouldThrowUserNotFoundException_WhenClientDoesNotExist()
        {
            // Arrange
            _clientsRepositoryMock.Setup(r => r.GetClientById(It.IsAny<int>())).ReturnsAsync((AbstractClient)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _contractService.CreateContract(new ContractDto(), 1));
        }

        [Fact]
        public async Task ProcessPayment_ShouldProcessPaymentSuccessfully()
        {
            // Arrange
            var contract = new SoftwareContract
            {
                Id = 1,
                ClientId = 1,
                Price = 3000,
                IsPaid = false,
                IsActive = true,
                EndDate = DateTime.Now.AddDays(5)
            };

            _softwareContractRepositoryMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync(contract);
            _paymentRepositoryMock.Setup(r => r.GetArleadyPayedForSoftware(It.IsAny<int>())).ReturnsAsync(0m);
            _paymentRepositoryMock.Setup(r => r.Add(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            _softwareContractRepositoryMock.Setup(r => r.Update(It.IsAny<SoftwareContract>()))
                .Returns(Task.CompletedTask);

            // Act
            var paymentDto = await _contractService.ProcessPayment(1, 3000);

            // Assert
            Assert.NotNull(paymentDto);
            Assert.True(contract.IsPaid);
            Assert.Equal(0, paymentDto.ToBePayed);
            _paymentRepositoryMock.Verify(r => r.Add(It.IsAny<Payment>()), Times.Once);
            _softwareContractRepositoryMock.Verify(r => r.Update(It.IsAny<SoftwareContract>()), Times.Once);
        }

        [Fact]
        public async Task ProcessPayment_ShouldThrowClientNotFoundException_WhenContractDoesNotExist()
        {
            // Arrange
            _softwareContractRepositoryMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((SoftwareContract)null);

            // Act & Assert
            await Assert.ThrowsAsync<ClientNotFoundException>(() => _contractService.ProcessPayment(1, 1000));
        }

        [Fact]
        public async Task ProcessPayment_ShouldThrowContractTimeToPayExceesException_WhenContractIsInactiveOrExpired()
        {
            // Arrange
            var contract = new SoftwareContract
            {
                Id = 1,
                ClientId = 1,
                IsPaid = false,
                IsActive = false,
                Price = 1000,
                StartDate = DateTime.Now.AddDays(-2),
                EndDate = DateTime.Now.AddDays(-1)
            };

            _softwareContractRepositoryMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync(contract);

            // Act & Assert
            await Assert.ThrowsAsync<ContractTimeToPayExceesException>(() => _contractService.ProcessPayment(1, 1000));
        }
    }
}