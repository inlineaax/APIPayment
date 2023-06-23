using Application.Models.Sale;
using Domain.Entities;

namespace Application.IService
{
    public interface ISaleService
    {
        ResponseSale RegisterSale(SalesData sales);
        List<Sale> GetSaleList();
        SaleDetails GetSale(int id);
        StatusChangeResponse UpdateSaleStatus(int saleId, SaleStatus newStatus);
    }
}
