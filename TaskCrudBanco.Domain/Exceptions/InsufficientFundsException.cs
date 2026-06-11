namespace TaskCrudBanco.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException()
        : base("Saldo insuficiente para realizar o saque.")
    {
    }
}
