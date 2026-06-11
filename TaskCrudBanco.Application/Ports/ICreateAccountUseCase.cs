using TaskCrudBanco.Application.Dto;

namespace TaskCrudBanco.Application.Ports;

public interface ICreateAccountUseCase
{
    Task<AccountResponseDto> ExecuteAsync(AccountRequestDto request);
}
