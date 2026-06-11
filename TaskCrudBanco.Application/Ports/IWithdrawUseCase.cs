using TaskCrudBanco.Application.Dto;

namespace TaskCrudBanco.Application.Ports;

public interface IWithdrawUseCase
{
    Task<TransactionResponseDto> ExecuteAsync(TransactionRequestDto request, Guid accountId);
}
