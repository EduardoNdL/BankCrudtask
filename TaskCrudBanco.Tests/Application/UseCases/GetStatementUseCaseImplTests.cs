using Moq;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Enums;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Tests.Application.UseCases;
public class GetStatementUseCaseImplTests
{
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly GetStatementUseCaseImpl _useCase;

    public GetStatementUseCaseImplTests()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _transactionRepository = new Mock<ITransactionRepository>();
        _useCase = new GetStatementUseCaseImpl(_accountRepository.Object, _transactionRepository.Object);
    }

    [Fact]
    public async Task GetStatement_ValidAccount_ReturnsStatementResponseWithNotEmptyList()
    {
        Account account = new Account("123", "Eduardo");
        Guid accountId = account.Id;
        List<Transaction> transactions = [
            new Transaction(accountId, TransactionType.Deposit, 100),
            new Transaction(accountId, TransactionType.Withdraw, 30),
        ];

        _accountRepository.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _transactionRepository.Setup(r => r.GetByAccountIdAsync(accountId, null, null)).ReturnsAsync(transactions);

        StatementResponseDto response = await _useCase.ExecuteAsync(accountId);

        Assert.Equal(account.AccountNumber, response.AccountResponseDto.AccountNumber);
        Assert.Equal(2, response.Transactions.Count());
    }

    [Fact]
    public async Task GetStatement_ValidAccount_ReturnsStatementResponseWithEmptyList()
    {
        Account account = new Account("123", "Eduardo");
        Guid accountId = account.Id;
        List<Transaction> transactions = [];

        _accountRepository.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _transactionRepository.Setup(r => r.GetByAccountIdAsync(accountId, null, null)).ReturnsAsync(transactions);

        StatementResponseDto response = await _useCase.ExecuteAsync(accountId);

        Assert.Equal(account.AccountNumber, response.AccountResponseDto.AccountNumber);
        Assert.Empty(response.Transactions);
    }

    [Fact]
    public async Task GetStatement_ValidAccountWithDateFilter_ReturnsFilteredTransactions()
    {
        Account account = new Account("123", "Eduardo");
        Guid accountId = account.Id;
        DateTime startDate = new DateTime(2026, 1, 1);
        DateTime endDate = new DateTime(2026, 1, 31);
        List<Transaction> transactions = [
            new Transaction(accountId, TransactionType.Deposit, 100),
        ];

        _accountRepository.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _transactionRepository.Setup(r => r.GetByAccountIdAsync(accountId, startDate, endDate)).ReturnsAsync(transactions);

        StatementResponseDto response = await _useCase.ExecuteAsync(accountId, startDate, endDate);

        Assert.Single(response.Transactions);
    }

    [Fact]
    public async Task GetStatement_InvalidAccount_ThrowsKeyNotFoundException()
    {
        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?) null);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _useCase.ExecuteAsync(Guid.NewGuid()));
    }
}
