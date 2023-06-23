using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IItemRepository
    {
        bool RegisterItem(Item item);
    }
}
