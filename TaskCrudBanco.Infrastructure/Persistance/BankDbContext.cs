using Microsoft.EntityFrameworkCore;
using TaskCrudBanco.Domain.Entities;

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
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
}