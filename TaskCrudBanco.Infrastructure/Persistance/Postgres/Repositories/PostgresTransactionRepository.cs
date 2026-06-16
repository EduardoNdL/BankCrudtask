using Microsoft.EntityFrameworkCore;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Infrastructure.Persistance.Postgres.Repositories;

public class PostgresTransactionRepository : ITransactionRepository
{

    private readonly BankDbContext _bankDbContext;

    public PostgresTransactionRepository (BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        await _bankDbContext.Transactions.AddAsync(transaction);
        await _bankDbContext.SaveChangesAsync();
        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _bankDbContext.Transactions.AsQueryable();

        query = query.Where(transaction => transaction.AccountId == accountId);

        if (startDate != null)
        {
            query = query.Where(transaction => transaction.CreatedAt >= startDate);
        }

        if (endDate != null)
        {
            query = query.Where(transaction => transaction.CreatedAt <= endDate);
        }

        return await query
                  .OrderBy(t => t.CreatedAt)
                  .ToListAsync();
    }
}