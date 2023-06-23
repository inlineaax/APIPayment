using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context
{
    public interface IDBPaymentContext
    {
        DbSet<Sale> Sale { get; set; }
        DbSet<Seller> Seller { get; set; }
        DbSet<Item> Item { get; set; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
