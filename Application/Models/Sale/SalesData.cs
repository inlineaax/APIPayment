using Application.Models.Item;
using Application.Models.Seller;

namespace Application.Models.Sale
{
    public class SalesData
    {
        public SellerData? Seller_Data { get; set; }
        public List<ItemData>? Item_Data { get; set; }

    }
}
