using Application.Models.Item;
using Application.Models.Seller;
using Domain.Entities;

namespace Application.Models.Sale
{
    public class SaleDetails
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public SaleStatus Status { get; set; }
        public SellerData? Seller { get; set; }
        public List<ItemData>? Items { get; set; }

    }
}
