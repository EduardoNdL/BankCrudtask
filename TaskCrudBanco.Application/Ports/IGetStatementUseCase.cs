using TaskCrudBanco.Application.Dto;

namespace TaskCrudBanco.Application.Ports;

public interface IGetStatementUseCase
{
    Task<StatementResponseDto> ExecuteAsync(Guid accountId, DateTime? startDate = null, DateTime? endDate = null);
}
