using Application.IService;
using Application.Models.Item;
using Application.Models.Sale;
using Application.Models.Seller;
using Application.Validators;
using Domain.Entities;
using Domain.IRepositories;
using SendGrid.Helpers.Errors.Model;

namespace Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IItemRepository _itemRepository;

        public SaleService(ISaleRepository saleRepository, ISellerRepository sellerRepository, IItemRepository itemRepository)
        {
            _saleRepository = saleRepository;
            _sellerRepository = sellerRepository;
            _itemRepository = itemRepository;
        }

        public ResponseSale RegisterSale(SalesData sales)
        {
            if (sales.Item_Data == null || sales.Item_Data.Count == 0)
            {
                throw new ArgumentException("At least one item is required to register a sale");
            }
            if (!SaleValidator.IsValidCPF(sales.Seller_Data.CPF))
            {
                throw new ArgumentException("Invalid CPF");
            }

            Sale newSale = new Sale
            {
                Date = DateTime.UtcNow,
                Status = SaleStatus.AwaitingPayment
            };

            _saleRepository.RegisterSale(newSale);

            Seller newSeller = new Seller
            {
                CPF = sales.Seller_Data?.CPF,
                Name = sales.Seller_Data?.Name,
                Email = sales.Seller_Data?.Email,
                Cell_Phone = sales.Seller_Data?.CellPhone,
                SaleId = newSale.Id
                
            };
            
            _sellerRepository.RegisterSeller(newSeller);

            foreach (var itemData in sales.Item_Data)
            {
                if (string.IsNullOrWhiteSpace(itemData.Item_Name))
                {
                    throw new ArgumentException("Item name cannot be empty");
                }

                Item newItem = new Item
                {
                    Item_Name = itemData.Item_Name,
                    SaleId = newSale.Id
                };
                _itemRepository.RegisterItem(newItem);
            }

            return new ResponseSale { Message = "Successfully registered sale" };
        }

        public List<Sale> GetSaleList()
        {
            try
            {
                var salesList = _saleRepository.GetSaleList().ToList();

                if (salesList.Count == 0)
                {
                    throw new NotFoundException("No sales found");
                }

                return salesList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching sales: " +  ex.Message);
            }
        }

        public SaleDetails GetSale(int id)
        {
            var sale = _saleRepository.GetSale(id);

            if (sale == null)
            {
                throw new NotFoundException($"Sale with ID {id} not found");
            }

            var seller = new SellerData
            {
                CPF = sale.Seller.CPF,
                Name = sale.Seller.Name,
                Email = sale.Seller.Email,
                CellPhone = sale.Seller.Cell_Phone
            };
            var Items = sale.Items.Select(i => new ItemData
            {
                Item_Name = i.Item_Name,
            }).ToList();

            var saleDetails = new SaleDetails
            {
                Date = sale.Date,
                Status = sale.Status,
                Id = id,
                Seller = seller,
                Items = Items,
            };

            return saleDetails;
        }

        public StatusChangeResponse UpdateSaleStatus(int saleId, SaleStatus newStatus)
        {
            var sale = _saleRepository.GetSale(saleId);

            if (sale == null)
            {
                throw new NotFoundException($"Sale with ID {saleId} not found");
            }

            if (!SaleValidator.IsValidStatusTransition(sale.Status, newStatus))
            {
                throw new ArgumentException("Invalid status transition");
            }

            string oldStatus = sale.Status.ToString();
            sale.Status = newStatus;
            _saleRepository.UpdateSale(sale);

            string message = $"Status changed from {oldStatus} to {newStatus}";

            return new StatusChangeResponse(oldStatus, newStatus.ToString(), message);

        }

    }
}
