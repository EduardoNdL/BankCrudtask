using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Application.UseCases;

public class GetBalanceUseCaseImpl : IGetBalanceUseCase
{
    private readonly IAccountRepository _accountRepository;

    public GetBalanceUseCaseImpl(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<decimal> ExecuteAsync(Guid accountId)
    {
        Account account = await _accountRepository.GetByIdAsync(accountId) 
            ?? throw new KeyNotFoundException("Account with this id not exists");

        return account.Balance;
    }
}