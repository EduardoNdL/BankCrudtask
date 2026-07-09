using TaskCrudBanco.Domain.Exceptions;
using TaskCrudBanco.Domain.ValueObjects;

namespace TaskCrudBanco.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; }
    public string OwnerName { get; private set; }

    public Money Balance { get; private set; } = Money.Zero;

    public DateTime CreatedAt { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    public Account()
    {
    }

    public Account(string accountNumber, string ownerName)
    {
        ArgumentException.ThrowIfNullOrEmpty(accountNumber);
        ArgumentException.ThrowIfNullOrEmpty(ownerName);
        
        this.Id = Guid.NewGuid();
        this.AccountNumber = accountNumber;
        this.OwnerName = ownerName;
        this.Balance = Money.Zero;
        this.CreatedAt = DateTime.Now;
    }

    public void Deposit(decimal amount)
    {
        this.Balance.Add(amount);
    } 

    public void Withdraw(decimal amount)
    {
        this.Balance.Subtract(amount);
    }
}
