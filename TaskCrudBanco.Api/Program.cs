using Microsoft.EntityFrameworkCore;
using TaskCrudBanco.Application.Ports;
using TaskCrudBanco.Application.UseCases;
using TaskCrudBanco.Domain.Ports;
using TaskCrudBanco.Infrastructure.Persistance;
using TaskCrudBanco.Infrastructure.Persistance.Postgres.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlite("Data Source=bank.db"));

builder.Services.AddScoped<IAccountRepository, PostgresAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, PostgresTransactionRepository>();

builder.Services.AddScoped<ICreateAccountUseCase, CreateAccountUseCaseImpl>();
builder.Services.AddScoped<IWithdrawUseCase, WithdrawUseCaseImpl>();
builder.Services.AddScoped<IDepositUseCase, DepositUseCaseImpl>();
builder.Services.AddScoped<IGetBalanceUseCase, GetBalanceUseCaseImpl>();
builder.Services.AddScoped<IGetStatementUseCase, GetStatementUseCaseImpl>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<BankDbContext>();
db.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
