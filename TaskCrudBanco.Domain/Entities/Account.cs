using TaskCrudBanco.Domain.Exceptions;

namespace TaskCrudBanco.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; }
    public string OwnerName { get; private set; }

    public decimal Balance { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    public Account()
    {
    }

    public Account(string accountNumber, string ownerName)
    {
        this.Id = Guid.NewGuid();
        this.AccountNumber = accountNumber;
        this.OwnerName = ownerName;
        this.Balance = 0;
        this.CreatedAt = DateTime.Now;
    }

    public void Deposit(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Value must be higher than zero");
        }
        this.Balance += amount;
    } 

    public void Withdraw(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Value must be higher than zero");
        }

        if(Balance < amount)
        {
            throw new InsufficientFundsException();
        }

        this.Balance -= amount;
    }
}
