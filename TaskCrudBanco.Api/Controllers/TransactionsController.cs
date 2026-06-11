using Microsoft.AspNetCore.Mvc;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;

namespace TaskCrudBanco.Api.Controllers;

[ApiController]
[Route("api/accounts/{accountId}/transactions/")]
public class TransactionsController : ControllerBase
{
    private readonly IDepositUseCase _deposit;
    private readonly IWithdrawUseCase _withdraw;


    public TransactionsController(IDepositUseCase deposit, IWithdrawUseCase withdraw)
    {
        _deposit = deposit;
        _withdraw = withdraw;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(Guid accountId, [FromBody] TransactionRequestDto request)
    {
        return Ok(await _deposit.ExecuteAsync(request, accountId));
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(Guid accountId, [FromBody] TransactionRequestDto request)
    {
        return Ok(await _withdraw.ExecuteAsync(request, accountId));
    }


}