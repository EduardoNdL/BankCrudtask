using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Domain.Entities;

public class Transaction
{

    public Guid Id { get; private set;}

    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = null!;

    public TransactionType Type { get; private set;}

    public decimal Amount { get; private set;}

    public DateTime CreatedAt { get; private set;}

    public Transaction()
    {
    }

    public Transaction(Guid accountId, TransactionType type, decimal amount)
    {
        this.Id = Guid.NewGuid();
        this.AccountId = accountId;
        this.Type = type;
        this.Amount = amount;
        this.CreatedAt = DateTime.UtcNow;
    }
}