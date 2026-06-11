namespace TaskCrudBanco.Application.Dto;

public record AccountRequestDto(
    string AccountNumber,
    string OwnerName
);