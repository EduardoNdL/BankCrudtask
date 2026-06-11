namespace TaskCrudBanco.Application.Dto;

public record StatementResponseDto(
    AccountResponseDto AccountResponseDto,
    IEnumerable<TransactionResponseDto> Transactions
);

