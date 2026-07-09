using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Application.Dto;

  public record TransactionResponseDto(
      Guid Id,
      TransactionType Type,
      decimal Amount,
      DateTime CreatedAt
  );
