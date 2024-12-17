using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Common;
using Payment.Domain.Models;
using Payment.Infrastructure.Configuration;

namespace Payment.Infrastructure
{
    public class PaymentDbContext : IdentityDbContext<User>
    {
        public PaymentDbContext(DbContextOptions options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<VirtualAccount> VirtualAccounts { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Payment.Domain.Models.Transaction> Transactions { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entity)
                {
                    switch (item.State)
                    {
                        case EntityState.Modified:
                            entity.UpdatedAt = DateTimeOffset.UtcNow;
                            break;
                        case EntityState.Added:
                            entity.CreatedAt = entity.UpdatedAt = DateTimeOffset.UtcNow;
                            break;
                        default:
                            break;
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }


}
