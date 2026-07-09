using TaskCrudBanco.Domain.Exceptions;
using TaskCrudBanco.Domain.ValueObjects;

namespace TaskCrudBanco.Tests.Domain;

public class MoneyTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Constructor_NegativeValue_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Money(-1));
        }

        [Fact]
        public void Constructor_ZeroValue_CreatesMoneyWithZeroValue()
        {
            Money money = new Money(0);

            Assert.Equal(0m, money.Value);
        }
    }

    public class AddTests
    {
        [Fact]
        public void Add_ValidAmount_IncreasesValue()
        {
            Money money = new Money(10);

            money.Add(5);

            Assert.Equal(15m, money.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Add_InvalidAmount_ThrowsArgumentException(decimal amount)
        {
            Money money = new Money(10);

            Assert.Throws<ArgumentException>(() => money.Add(amount));
        }
    }

    public class SubtractTests
    {
        [Fact]
        public void Subtract_ValidAmount_DecreasesValue()
        {
            Money money = new Money(10);

            money.Subtract(5);

            Assert.Equal(5m, money.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Subtract_InvalidAmount_ThrowsArgumentException(decimal amount)
        {
            Money money = new Money(10);

            Assert.Throws<ArgumentException>(() => money.Subtract(amount));
        }

        [Fact]
        public void Subtract_InsufficientFunds_ThrowsInsufficientFundsException()
        {
            Money money = new Money(10);

            Assert.Throws<InsufficientFundsException>(() => money.Subtract(20));
        }
    }
}
