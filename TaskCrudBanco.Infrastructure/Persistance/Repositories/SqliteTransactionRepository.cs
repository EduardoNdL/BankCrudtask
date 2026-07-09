using Microsoft.EntityFrameworkCore;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Infrastructure.Persistance.Repositories;

public class SqliteTransactionRepository : ITransactionRepository
{
    private readonly BankDbContext _bankDbContext;

    public SqliteTransactionRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        await _bankDbContext.Transactions.AddAsync(transaction);
        await _bankDbContext.SaveChangesAsync();
        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(
        Guid accountId,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _bankDbContext.Transactions
          .Where(t => t.AccountId == accountId);

        if (startDate.HasValue)
          query = query.Where(t => t.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
          query = query.Where(t => t.CreatedAt < endDate.Value.Date.AddDays(1));

        return await query
          .OrderBy(t => t.CreatedAt)
          .ToListAsync();
    }
}
