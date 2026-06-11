using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.UseCases;

public class GetStatementUseCaseImpl : IGetStatementUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GetStatementUseCaseImpl(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<StatementResponseDto> ExecuteAsync(Guid accountId, DateTime? startDate = null, DateTime? endDate = null)
    {
        Account account = await _accountRepository.GetByIdAsync(accountId) 
            ?? throw new KeyNotFoundException("Account with this id not exists");

        AccountResponseDto accountResponseDto = new AccountResponseDto(account.Id, account.AccountNumber,
            account.OwnerName, account.Balance);

        List<TransactionResponseDto> transactions = (await _transactionRepository.GetByAccountIdAsync(accountId, startDate, endDate))
            .Select(t => new TransactionResponseDto(t.Id, t.Type, t.Amount, t.CreatedAt))
            .ToList();

        return new StatementResponseDto(accountResponseDto, transactions);        
    }
}