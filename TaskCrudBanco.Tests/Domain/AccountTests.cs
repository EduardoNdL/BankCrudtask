using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Exceptions;

namespace TaskCrudBanco.Tests.Domain;

public class AccountTests
{
    public class ConstructorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_NullOrEmptyAccountNumber_ThrowsArgumentException(string accountNumber)
        {
            Assert.ThrowsAny<ArgumentException>(() => new Account(accountNumber, "Eduardo"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_NullOrEmptyOwnerName_ThrowsArgumentException(string ownerName)
        {
            Assert.ThrowsAny<ArgumentException>(() => new Account("123", ownerName));
        }
    }

    public class DepositTests
    {
        private readonly Account _account;

        public DepositTests()
        {
            _account = new Account("123", "Eduardo");
        }

        [Fact]
        public void Deposit_ValidAmount_IncreasesBalance()
        {
            _account.Deposit(10);
        
            Assert.Equal(10m, _account.Balance);
        }

    }

    public class WithdrawTests
    {

        private readonly Account _account;

        public WithdrawTests()
        {
            _account = new Account("123", "Eduardo");
        }

        [Fact]
        public void Withdraw_ValidAmount_DecreasesBalance()
        {
            _account.Deposit(10);
        
            _account.Withdraw(5);
        
            Assert.Equal(5m, _account.Balance);
        }

    }
}