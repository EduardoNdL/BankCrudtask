using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.Strategies;

public class DepositStrategy : ITransactionStrategy
{
    public TransactionType Type => TransactionType.Deposit;

    public void Apply(Account account, decimal amount){
        account.Deposit(amount);
    }
  }
