using System.ComponentModel.DataAnnotations;

namespace TaskCrudBanco.Application.Dto;

public record AccountRequestDto(
    [Required] [MinLength(4)] [MaxLength(20)] string AccountNumber,
    [Required] [MinLength(2)] [MaxLength(100)] string OwnerName
);