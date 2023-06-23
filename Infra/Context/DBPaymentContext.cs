using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Infra.Context
{
    public class DBPaymentContext : DbContext , IDBPaymentContext
    {
        protected readonly IConfiguration Configuration;

        public DBPaymentContext(DbContextOptions<DBPaymentContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sale { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Item> Item { get; set; }
        public IDbContextTransaction? Transaction { get; private set; }

        public IDbContextTransaction BeginTransaction()
        {
            if (Transaction == null)
                Transaction = this.Database.BeginTransaction();

            return Transaction;
        }
        public async Task<int> SaveChangesAsync()
        {
            var save = await base.SaveChangesAsync();
            await CommitAsync();
            return save;
        }

        public override int SaveChanges()
        {
            var save = base.SaveChanges();
            Commit();
            return save;
        }

        internal void RollBack()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }
        private async Task CommitAsync()
        {
            if (Transaction != null)
            {
                await Transaction.CommitAsync();
                await Transaction.DisposeAsync();
                Transaction = null;
            }
        }
        private void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

    }
}
