using TaskCrudBanco.Application.Dto;

namespace TaskCrudBanco.Application.Ports;

public interface IGetBalanceUseCase
{
    Task<decimal> ExecuteAsync(Guid accountId);
}
