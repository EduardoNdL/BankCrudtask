using Moq;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Application.Strategies;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Enums;
using TaskCrudBanco.Domain.Exceptions;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Tests.Application.UseCases;

public class TransactionUseCaseImplTests
{
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly TransactionUseCaseImpl _useCase;

    public TransactionUseCaseImplTests()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _transactionRepository = new Mock<ITransactionRepository>();

        IEnumerable<ITransactionStrategy> strategies = new List<ITransactionStrategy>
        {
            new DepositStrategy(),
            new WithdrawStrategy()
        };

        _useCase = new TransactionUseCaseImpl(
            _accountRepository.Object,
            _transactionRepository.Object,
            strategies);
    }

    [Fact]
    public async Task ExecuteAsync_Deposit_ReturnsTransactionResponse()
    {
        TransactionRequestDto request = new TransactionRequestDto(100);
        Account account = new Account("123", "Eduardo");

        _accountRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);
        _transactionRepository.Setup(repository => repository.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction t) => t);

        TransactionResponseDto response = await _useCase.ExecuteAsync(request, Guid.NewGuid(), TransactionType.Deposit);

        Assert.Equal(request.Amount, response.Amount);
        Assert.Equal(TransactionType.Deposit, response.Type);
    }

    [Fact]
    public async Task ExecuteAsync_Withdraw_ReturnsTransactionResponse()
    {
        TransactionRequestDto request = new TransactionRequestDto(50);
        Account account = new Account("123", "Eduardo");
        account.Deposit(100);

        _accountRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);
        _transactionRepository.Setup(repository => repository.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction t) => t);

        TransactionResponseDto response = await _useCase.ExecuteAsync(request, Guid.NewGuid(), TransactionType.Withdraw);

        Assert.Equal(request.Amount, response.Amount);
        Assert.Equal(TransactionType.Withdraw, response.Type);
    }

    [Fact]
    public async Task ExecuteAsync_AccountNotFound_ThrowsKeyNotFoundException()
    {
        _accountRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _useCase.ExecuteAsync(new TransactionRequestDto(10), Guid.NewGuid(), TransactionType.Deposit));
    }

    [Fact]
    public async Task ExecuteAsync_UnknownType_ThrowsNotSupportedException()
    {
        Account account = new Account("123", "Eduardo");
        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);

        await Assert.ThrowsAsync<NotSupportedException>(
            () => _useCase.ExecuteAsync(new TransactionRequestDto(10), Guid.NewGuid(), (TransactionType)99));
    }

    [Fact]
    public async Task ExecuteAsync_InsufficientFunds_ThrowsInsufficientFundsException()
    {
        Account account = new Account("123", "Eduardo");
        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);

        await Assert.ThrowsAsync<InsufficientFundsException>(
            () => _useCase.ExecuteAsync(new TransactionRequestDto(100), Guid.NewGuid(), TransactionType.Withdraw));
    }
}
