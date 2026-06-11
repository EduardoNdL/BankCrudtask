using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.UseCases;

public class WithdrawUseCaseImpl : IWithdrawUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public WithdrawUseCaseImpl(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionResponseDto> ExecuteAsync(TransactionRequestDto request, Guid accountId)
    {
        Account account = await _accountRepository.GetByIdAsync(accountId) 
            ?? throw new KeyNotFoundException("Account with this id not exists");
        
        Transaction transaction = new Transaction(accountId, TransactionType.Withdraw, request.Amount);

        account.Withdraw(transaction.Amount);
        Transaction response = await _transactionRepository.AddAsync(transaction);

        return new TransactionResponseDto(response.Id, response.Type, response.Amount, response.CreatedAt);
    }
}