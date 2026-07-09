using Microsoft.AspNetCore.Mvc;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;

namespace TaskCrudBanco.Api.Controllers;

[ApiController]
[Route("api/accounts/")]
public class AccountsController : ControllerBase
{
    private readonly ICreateAccountUseCase _createAccount;
    private readonly IGetBalanceUseCase _getBalance;
    private readonly IGetStatementUseCase _getStatement;

    public AccountsController(
        ICreateAccountUseCase createAccount,
        IGetBalanceUseCase getBalance,
        IGetStatementUseCase getStatement)
    {
        _createAccount = createAccount;
        _getBalance = getBalance;
        _getStatement = getStatement;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] AccountRequestDto requestDto)
    {
        return Created((string?)null, await _createAccount.ExecuteAsync(requestDto));
    }

    [HttpGet("{accountId}/balance")]
    public async Task<IActionResult> GetBalance([FromRoute] Guid accountId)
    {
        return Ok(await _getBalance.ExecuteAsync(accountId));
    }

    [HttpGet("{accountId}/statement")]
    public async Task<IActionResult> GetStatement(
        [FromRoute] Guid accountId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        return Ok(await _getStatement.ExecuteAsync(accountId, startDate, endDate));
    }

}