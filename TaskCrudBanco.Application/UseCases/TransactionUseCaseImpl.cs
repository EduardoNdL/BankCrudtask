using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Enums;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Application.UseCases;

public class TransactionUseCaseImpl(
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository,
    IEnumerable<ITransactionStrategy> strategies) : ITransactionUseCase
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    private readonly IEnumerable<ITransactionStrategy> _strategies = strategies;

    public async Task<TransactionResponseDto> ExecuteAsync(TransactionRequestDto request, Guid accountId, TransactionType type)
    {
        Account account = await _accountRepository.GetByIdAsync(accountId)
            ?? throw new KeyNotFoundException("Account with this id not exists");

        ITransactionStrategy strategy = _strategies
            .FirstOrDefault(t => t.Type == type)
            ?? throw new NotSupportedException($"Transaction with type {type} not supported");

        Transaction transaction = new Transaction(account.Id, strategy.Type, request.Amount);

        strategy.Apply(account, request.Amount);
        await _accountRepository.UpdateAsync(account);

        Transaction response = await _transactionRepository.AddAsync(transaction);
        
        return new TransactionResponseDto(
            response.Id,
            response.Type,
            transaction.Amount,
            transaction.CreatedAt);
    }
}