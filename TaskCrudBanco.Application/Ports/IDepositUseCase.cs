using TaskCrudBanco.Application.Dto;

namespace TaskCrudBanco.Application.Ports;

public interface IDepositUseCase
{
    Task<TransactionResponseDto> ExecuteAsync(TransactionRequestDto request, Guid accountId);
}
