using Domain.Entities;
using Domain.IRepositories;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly IDBPaymentContext _context;

        public SaleRepository(IDBPaymentContext context) => _context = context;

        public bool RegisterSale(Sale sale)
        {
            _context.Sale.Add(sale);
            _context.SaveChanges();

            return true;
        }

        public List<Sale> GetSaleList()
        {
            var sales = _context.Sale.Include(x => x.Seller).Include(x => x.Items).ToList();

            if (sales.Count == 0)
            {
                throw new ArgumentException("No registered sales found");
            }

            return sales;
        }

        public Sale GetSale(int id)
        {
            var sale = _context.Sale.Include(x => x.Seller).Include(x => x.Items).FirstOrDefault(x => x.Id == id);

            if (sale == null)
            {
                throw new ArgumentException($"Sale with Id {id} not found");
            }

            return sale;
        }

        public bool UpdateSale(Sale sale)
        {
            _context.SaveChanges();

            return true;
        }
    }
}
