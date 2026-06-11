using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.UseCases;

public class DepositUseCaseImpl : IDepositUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public DepositUseCaseImpl(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionResponseDto> ExecuteAsync(TransactionRequestDto request, Guid accountId)
    {
        Account account = await _accountRepository.GetByIdAsync(accountId) 
            ?? throw new KeyNotFoundException("Account with this id not exists");
        
        Transaction transaction = new Transaction(accountId, TransactionType.Deposit, request.Amount);

        account.Deposit(transaction.Amount);
        await _accountRepository.UpdateAsync(account);
        Transaction response = await _transactionRepository.AddAsync(transaction);

        return new TransactionResponseDto(response.Id, response.Type, response.Amount, response.CreatedAt);
    }
}