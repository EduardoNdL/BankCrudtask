namespace TaskCrudBanco.Application.Dto;

public record AccountResponseDto(
      Guid Id,
      string AccountNumber,
      string OwnerName,
      decimal Balance
  );
