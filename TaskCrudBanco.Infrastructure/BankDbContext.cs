using Microsoft.EntityFrameworkCore;
using TaskCrudBanco.Domain.Entities;
using TaskCrudBanco.Domain.ValueObjects;

namespace TaskCrudBanco.Infrastructure.Persistance;

public class BankDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    
    protected BankDbContext()
    {
    }
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId);

        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasConversion(
                money => money.Value,
                value => new Money(value))
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
}