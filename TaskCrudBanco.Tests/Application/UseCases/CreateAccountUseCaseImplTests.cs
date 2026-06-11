using Moq;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Tests.Application.UseCases;
public class CreateAccountUseCaseImplTests
{
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly CreateAccountUseCaseImpl _useCase;

    public CreateAccountUseCaseImplTests()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _useCase = new CreateAccountUseCaseImpl(_accountRepository.Object);
    }

    [Fact]
    public async Task CreateAccount_ValidRequest_ReturnsAccountResponse()
    {
        AccountRequestDto request = new AccountRequestDto("123", "Eduardo");

        _accountRepository.Setup(r => r.GetByNumberAsync(request.AccountNumber)).ReturnsAsync((Account?) null);
        _accountRepository.Setup(r => r.AddAsync(It.IsAny<Account>())).ReturnsAsync((Account a) => a);

        AccountResponseDto response = await _useCase.ExecuteAsync(request);

        Assert.Equal(request.AccountNumber, response.AccountNumber);
        Assert.Equal(request.OwnerName, response.OwnerName);
        Assert.Equal(0, response.Balance);
    }

    [Fact]
    public async Task CreateAccount_DuplicateAccountNumber_ThrowsArgumentException()
    {
        AccountRequestDto request = new AccountRequestDto("123", "Eduardo");
        Account existingAccount = new Account(request.AccountNumber, request.OwnerName);

        _accountRepository.Setup(r => r.GetByNumberAsync(request.AccountNumber)).ReturnsAsync(existingAccount);

        await Assert.ThrowsAsync<ArgumentException>(async () => await _useCase.ExecuteAsync(request));
    }
}
