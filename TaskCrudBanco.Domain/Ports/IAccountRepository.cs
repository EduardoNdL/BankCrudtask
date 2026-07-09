using TaskCrudBanco.Domain.Entities;

namespace TaskCrudBanco.Domain.Ports;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);   
    Task<Account?> GetByNumberAsync(string number);

    Task<Account> AddAsync(Account account);
    Task UpdateAsync(Account account);
}