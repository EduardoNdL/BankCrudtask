using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.Ports;

public interface ITransactionStrategy
{
    TransactionType Type { get; }
    void Apply(Account account, decimal amount);
}
