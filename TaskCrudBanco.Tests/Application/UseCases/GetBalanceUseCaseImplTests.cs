using Moq;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Tests.Application.UseCases;
public class GetBalanceUseCaseImplTests
{
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly GetBalanceUseCaseImpl _useCase;

    public GetBalanceUseCaseImplTests()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _useCase = new GetBalanceUseCaseImpl(_accountRepository.Object);
    }

    [Fact]
    public async Task GetBalance_ValidAccount_ReturnsBalance()
    {
        Account account = new Account("123", "Eduardo");
        account.Deposit(50);

        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);

        decimal balance = await _useCase.ExecuteAsync(Guid.NewGuid());

        Assert.Equal(account.Balance, balance);
    }

    [Fact]
    public async Task GetBalance_InvalidAccount_ThrowsKeyNotFoundException()
    {
        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?) null);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _useCase.ExecuteAsync(Guid.NewGuid()));
    }
}
