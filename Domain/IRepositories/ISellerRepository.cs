using Domain.Entities;

namespace Domain.IRepositories
{
    public interface ISellerRepository
    {
        bool RegisterSeller(Seller seller);
    }
}
