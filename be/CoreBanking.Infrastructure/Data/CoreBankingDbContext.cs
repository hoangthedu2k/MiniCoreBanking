using CoreBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreBanking.Infrastructure.Data
{
    public class CoreBankingDbContext : DbContext
    {
        public CoreBankingDbContext(DbContextOptions<CoreBankingDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Corrected with 'override' keyword  
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình quan hệ 1-N giữa Customer và Account  
            modelBuilder.Entity<Account>(
                entity =>
                {
                    entity.HasKey(a => a.AccountNumber);
                    entity.Property(a => a.Balance).HasColumnType("decimal(18,2)");
                    entity.Property(a => a.AccountType).HasConversion<string>();
                }
            );
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
                entity.Property(t => t.TransactionType).HasConversion<string>();
                entity.Property(t => t.Status).HasConversion<string>();
            });
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(c => c.Email).IsUnique();
            });
        }
    }
}
