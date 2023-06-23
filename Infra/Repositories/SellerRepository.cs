using Domain.Entities;
using Domain.IRepositories;
using Infra.Context;

namespace Infra.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly IDBPaymentContext _context;

        public SellerRepository(IDBPaymentContext context) => _context = context;

        public bool RegisterSeller(Seller seller)
        {
            _context.Seller.Add(seller);
            _context.SaveChanges();

            return true;
        }
    }
}
