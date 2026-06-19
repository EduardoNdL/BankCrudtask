using Microsoft.AspNetCore.Mvc;
using TaskCrudBanco.Application.Dto;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Domain.Enums;

namespace TaskCrudBanco.Api.Controllers;

[ApiController]
[Route("api/accounts/{accountId}/transactions/")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionUseCase _transactionUseCase;


    public TransactionsController(ITransactionUseCase transactionUseCase)
    {
        _transactionUseCase = transactionUseCase;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(Guid accountId, [FromBody] TransactionRequestDto request)
    {
        return Ok(await _transactionUseCase.ExecuteAsync(request, accountId, TransactionType.Deposit));
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(Guid accountId, [FromBody] TransactionRequestDto request)
    {
        return Ok(await _transactionUseCase.ExecuteAsync(request, accountId, TransactionType.Withdraw));
    }


}