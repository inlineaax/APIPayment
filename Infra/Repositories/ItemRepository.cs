using Domain.Entities;
using Domain.IRepositories;
using Infra.Context;

namespace Infra.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDBPaymentContext _context;

        public ItemRepository(IDBPaymentContext context) => _context = context;

        public bool RegisterItem(Item item)
        {
            _context.Item.Add(item);
            _context.SaveChanges();

            return true;
        }
    }
}
