using Microsoft.EntityFrameworkCore;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Infrastructure.Persistance.Repositories;

public class SqliteAccountRepository : IAccountRepository
{
    private readonly BankDbContext _bankDbContext;

    public SqliteAccountRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public async Task<Account> AddAsync(Account account)
    {
        await _bankDbContext.Accounts.AddAsync(account);
        await _bankDbContext.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _bankDbContext.Accounts.FindAsync(id);
    }

    public async Task<Account?> GetByNumberAsync(string number)
    {
        return await _bankDbContext.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == number);
    }

    public async Task UpdateAsync(Account account)
    {
        _bankDbContext.Accounts.Update(account);
        await _bankDbContext.SaveChangesAsync();
    }
}