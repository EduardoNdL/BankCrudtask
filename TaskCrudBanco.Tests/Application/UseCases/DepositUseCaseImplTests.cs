using Moq;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Tests.Application.UseCases;
public class DepositUseCaseImplTests
{
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly DepositUseCaseImpl _useCase;

    public DepositUseCaseImplTests()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _transactionRepository = new Mock<ITransactionRepository>();
        _useCase = new DepositUseCaseImpl(_accountRepository.Object, _transactionRepository.Object);
    }

    [Fact]
    public async Task Deposit_ValidAmountAndAccount_ReturnsTransactionResponse()
    {
        TransactionRequestDto transactionRequestDto = new TransactionRequestDto(10);
        Account account = new Account("123", "Eduardo");

        _accountRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);
        _transactionRepository.Setup(repository => repository.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction transaction) => transaction);

        TransactionResponseDto response = await _useCase.ExecuteAsync(transactionRequestDto, Guid.NewGuid());

        Assert.Equal(transactionRequestDto.Amount, response.Amount);
    }

    [Fact]
    public async Task Deposit_InvalidAccount_ThrowKeyNotFoundException()
    {
        TransactionRequestDto transactionRequestDto = new TransactionRequestDto(10);

        _accountRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?) null);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _useCase.ExecuteAsync(transactionRequestDto, Guid.NewGuid()));
    }
}
