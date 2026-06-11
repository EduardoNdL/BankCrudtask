using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.Exceptions;

namespace TaskCrudBanco.Tests.Domain;

public class AccountTests
{
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
        
            Assert.Equal(10, _account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Deposit_InvalidAmount_ThrowsArgumentException(decimal amount)
        {
            Assert.Throws<ArgumentException>(() => _account.Deposit(amount));
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
        
            Assert.Equal(5, _account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Withdraw_InvalidAmount_ThrowsArgumentException(decimal amount)
        {
            Assert.Throws<ArgumentException>(() => _account.Withdraw(amount));
        }

        [Fact]
        public void Withdraw_InsufficientBalance_ThrowsInsufficientFundsException()
        {
            Assert.Throws<InsufficientFundsException>(() => _account.Withdraw(5));
        }
    }
}