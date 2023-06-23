using Domain.Entities;

namespace Domain.IRepositories
{
    public interface ISaleRepository
    {
        bool RegisterSale(Sale sale);
        List<Sale> GetSaleList();
        Sale GetSale(int id);
        bool UpdateSale(Sale sale);
    }
}
