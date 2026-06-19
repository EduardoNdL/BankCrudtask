using System.ComponentModel.DataAnnotations;

namespace TaskCrudBanco.Application.Dto;

public record TransactionRequestDto(
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")] decimal Amount
);