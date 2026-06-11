using Moq;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Tests.Application.UseCases;
public class WithdrawUseCaseImplTests
{
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly WithdrawUseCaseImpl _useCase;


    public WithdrawUseCaseImplTests()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _transactionRepository = new Mock<ITransactionRepository>();
        _useCase = new WithdrawUseCaseImpl(_accountRepository.Object, _transactionRepository.Object);
    }

    [Fact]
    public async Task Withdraw_ValidAmountAndAccount_ReturnsTransactionResponse()
    {
        TransactionRequestDto transactionRequestDto = new TransactionRequestDto(10);
        Account account = new Account("123", "Eduardo");
        account.Deposit(15);
        
        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);
        _transactionRepository.Setup(r => r.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction t) => t);

        TransactionResponseDto response = await _useCase.ExecuteAsync(transactionRequestDto, Guid.NewGuid());

        Assert.Equal(transactionRequestDto.Amount, response.Amount);
    }

    [Fact]
    public async Task Withdraw_InvalidAccount_ThrowKeyNotFoundException()
    {
        TransactionRequestDto transactionRequestDto = new TransactionRequestDto(10);

        _accountRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?) null);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _useCase.ExecuteAsync(transactionRequestDto, Guid.NewGuid()));
    }
}