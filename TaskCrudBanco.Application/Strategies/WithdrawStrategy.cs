using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.Strategies;

public class WithdrawStrategy : ITransactionStrategy
{
    public TransactionType Type => TransactionType.Withdraw;

    public void Apply(Account account, decimal amount){
        account.Withdraw(amount);
    }
  }
