using TaskCrudBanco.Domain.Exceptions;

namespace TaskCrudBanco.Domain.ValueObjects;

public class Money
{
    public decimal Value { get; private set; }

    public static Money Zero => new Money(0);

    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Value cannot be negative");
        Value = value;
    }

    public void Add(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Value must be higher than zero");
        Value += amount;
    }

    public void Subtract(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Value must be higher than zero");
        if (Value < amount)
            throw new InsufficientFundsException();
        Value -= amount;
    }

    public static implicit operator decimal(Money money){
        return money.Value;
    }
}
