using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.Ports;

public interface ITransactionUseCase
{
    Task<TransactionResponseDto> ExecuteAsync(
        TransactionRequestDto request,
        Guid accountId,
        TransactionType type);
}