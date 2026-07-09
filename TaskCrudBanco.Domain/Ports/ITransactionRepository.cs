using TaskCrudBanco.Domain.Entities;

namespace TaskCrudBanco.Domain.Ports;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, DateTime? startDate = null, DateTime? endDate = null);
}