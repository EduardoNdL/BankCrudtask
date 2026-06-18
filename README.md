# BankCrudtask

API RESTful bancária para gerenciamento de contas e transações financeiras, construída com .NET 10 seguindo arquitetura hexagonal.

## Tecnologias

- .NET 10 / ASP.NET Core
- Entity Framework Core + SQLite
- xUnit + Moq (testes unitários)
- Swagger / OpenAPI

## Arquitetura

O projeto segue o padrão de **Arquitetura Hexagonal (Ports & Adapters)**, separado em quatro camadas:

```
TaskCrudBanco.Domain          # Entidades, Enums, value objects, ports (repositórios) e exceções de domínio
TaskCrudBanco.Application     # Use cases e DTOs
TaskCrudBanco.Infrastructure  # Repositórios (SQLite), DbContext e migrations
TaskCrudBanco.Api             # Controllers, configuração de injeções de dependências e startup
TaskCrudBanco.Tests           # Testes unitários dos use cases e domínio
```

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Como rodar

```bash
# 1, Clonar o repositório
git clone <url-do-repositorio>
cd BankCrudtask

# 2. Executar a API (as migrations pendentes são aplicadas no startup)
dotnet run --project TaskCrudBanco.Api
```

> Para criar novas migrations após alterar entidades:
> ```bash
> dotnet ef migrations add <NomeDaMigration> --project TaskCrudBanco.Infrastructure --startup-project TaskCrudBanco.Api
> ```

A API estará disponível em `http://localhost:5000`.
Swagger UI: `http://localhost:5000/swagger`

## Endpoints

### Contas

| Método | Rota                              | Descrição                                 |
|--------|-----------------------------------|-------------------------------------------|
| POST   | `/api/accounts`                   | Criar nova conta                          |
| GET    | `/api/accounts/{id}/balance`      | Consultar saldo                           |
| GET    | `/api/accounts/{id}/statement`    | Extrato (filtros: `startDate`, `endDate`) |

### Transações

| Método | Rota                                          | Descrição |
|--------|-----------------------------------------------|-----------|
| POST   | `/api/accounts/{id}/transactions/deposit`     | Depósito  |
| POST   | `/api/accounts/{id}/transactions/withdraw`    | Saque     |

## Diagramas de Sequência

### Criar Conta

```mermaid
sequenceDiagram
    actor Client
    participant AccountsController
    participant CreateAccountUseCase
    participant AccountRepository
    participant Database

    Client->>AccountsController: POST /api/accounts
    AccountsController->>CreateAccountUseCase: ExecuteAsync(request)
    CreateAccountUseCase->>AccountRepository: GetByNumberAsync(accountNumber)
    AccountRepository->>Database: SELECT
    Database-->>AccountRepository: null
    AccountRepository-->>CreateAccountUseCase: null
    CreateAccountUseCase->>AccountRepository: AddAsync(account)
    AccountRepository->>Database: INSERT
    Database-->>AccountRepository: account
    AccountRepository-->>CreateAccountUseCase: account
    CreateAccountUseCase-->>AccountsController: AccountResponseDto
    AccountsController-->>Client: 201 Created
```

### Depósito

```mermaid
sequenceDiagram
    actor Client
    participant TransactionsController
    participant DepositUseCase
    participant AccountRepository
    participant TransactionRepository
    participant Database

    Client->>TransactionsController: POST /api/accounts/{id}/transactions/deposit
    TransactionsController->>DepositUseCase: ExecuteAsync(request, accountId)
    DepositUseCase->>AccountRepository: GetByIdAsync(accountId)
    AccountRepository->>Database: SELECT
    Database-->>AccountRepository: account
    AccountRepository-->>DepositUseCase: account
    DepositUseCase->>DepositUseCase: account.Deposit(amount)
    DepositUseCase->>AccountRepository: UpdateAsync(account)
    AccountRepository->>Database: UPDATE
    DepositUseCase->>TransactionRepository: AddAsync(transaction)
    TransactionRepository->>Database: INSERT
    Database-->>TransactionRepository: transaction
    TransactionRepository-->>DepositUseCase: transaction
    DepositUseCase-->>TransactionsController: TransactionResponseDto
    TransactionsController-->>Client: 200 OK
```

### Saque

```mermaid
sequenceDiagram
    actor Client
    participant TransactionsController
    participant WithdrawUseCase
    participant AccountRepository
    participant TransactionRepository
    participant Database

    Client->>TransactionsController: POST /api/accounts/{id}/transactions/withdraw
    TransactionsController->>WithdrawUseCase: ExecuteAsync(request, accountId)
    WithdrawUseCase->>AccountRepository: GetByIdAsync(accountId)
    AccountRepository->>Database: SELECT
    Database-->>AccountRepository: account
    AccountRepository-->>WithdrawUseCase: account
    WithdrawUseCase->>WithdrawUseCase: account.Withdraw(amount)
    Note over WithdrawUseCase: lança InsufficientFundsException<br/>se saldo insuficiente
    WithdrawUseCase->>AccountRepository: UpdateAsync(account)
    AccountRepository->>Database: UPDATE
    WithdrawUseCase->>TransactionRepository: AddAsync(transaction)
    TransactionRepository->>Database: INSERT
    Database-->>TransactionRepository: transaction
    TransactionRepository-->>WithdrawUseCase: transaction
    WithdrawUseCase-->>TransactionsController: TransactionResponseDto
    TransactionsController-->>Client: 200 OK
```

### Consultar Saldo

```mermaid
sequenceDiagram
    actor Client
    participant AccountsController
    participant GetBalanceUseCase
    participant AccountRepository
    participant Database

    Client->>AccountsController: GET /api/accounts/{id}/balance
    AccountsController->>GetBalanceUseCase: ExecuteAsync(accountId)
    GetBalanceUseCase->>AccountRepository: GetByIdAsync(accountId)
    AccountRepository->>Database: SELECT
    Database-->>AccountRepository: account
    AccountRepository-->>GetBalanceUseCase: account
    GetBalanceUseCase-->>AccountsController: decimal (balance)
    AccountsController-->>Client: 200 OK
```

### Extrato

```mermaid
sequenceDiagram
    actor Client
    participant AccountsController
    participant GetStatementUseCase
    participant AccountRepository
    participant TransactionRepository
    participant Database

    Client->>AccountsController: GET /api/accounts/{id}/statement?startDate&endDate
    AccountsController->>GetStatementUseCase: ExecuteAsync(accountId, startDate, endDate)
    GetStatementUseCase->>AccountRepository: GetByIdAsync(accountId)
    AccountRepository->>Database: SELECT
    Database-->>AccountRepository: account
    AccountRepository-->>GetStatementUseCase: account
    GetStatementUseCase->>TransactionRepository: GetByAccountIdAsync(accountId, startDate, endDate)
    TransactionRepository->>Database: SELECT (filtrado ou não por data)
    Database-->>TransactionRepository: transactions
    TransactionRepository-->>GetStatementUseCase: transactions
    GetStatementUseCase-->>AccountsController: StatementResponseDto
    AccountsController-->>Client: 200 OK
```

## Testes

```bash
dotnet test
```

Os testes cobrem use cases da camada de aplicação e regras de domínio (`Account`, `Money`), utilizando mocks com Moq.
