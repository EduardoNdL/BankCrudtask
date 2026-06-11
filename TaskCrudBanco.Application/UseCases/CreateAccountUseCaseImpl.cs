using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Ports;

namespace TaskCrudBanco.Application.UseCases;

public class CreateAccountUseCaseImpl : ICreateAccountUseCase
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountUseCaseImpl(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponseDto> ExecuteAsync(AccountRequestDto request)
    {
        if(await _accountRepository.GetByNumberAsync(request.AccountNumber) != null)
        {
            throw new ArgumentException("Account with this number already exists");
        }

        Account account = new Account(request.AccountNumber, request.OwnerName);
        
        Account response = await _accountRepository.AddAsync(account);

        return new AccountResponseDto(response.Id, response.AccountNumber, response.OwnerName, response.Balance);
    }
}