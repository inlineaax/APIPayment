using Application.Models.Item;
using Application.Models.Sale;
using Application.Models.Seller;
using Application.Services;
using Domain.Entities;
using Domain.IRepositories;
using FluentAssertions;
using Moq;
using SendGrid.Helpers.Errors.Model;

namespace ApiPayment.Tests
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock = new Mock<ISaleRepository>();
        private readonly Mock<ISellerRepository> _sellerRepositoryMock = new Mock<ISellerRepository>();
        private readonly Mock<IItemRepository> _itemRepositoryMock = new Mock<IItemRepository>();

        public SaleServiceTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _sellerRepositoryMock = new Mock<ISellerRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
        }

        #region RegisterSale
        [Fact]
        public void RegisterSale_WithValidData_ReturnsSuccessMessage()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "12345678909",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>
            {
                new ItemData { Item_Name = "Item 1" },
                new ItemData { Item_Name = "Item 2" }
            }
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.RegisterSale(salesData);

            // Assert
            result.Message.Should().Be("Successfully registered sale");
        }

        [Fact]
        public void RegisterSale_WithValidCPF_ReturnsSuccessMessage()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "12345678909",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>
        {
            new ItemData { Item_Name = "Item 1" },
            new ItemData { Item_Name = "Item 2" }
        }
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.RegisterSale(salesData);

            // Assert
            result.Message.Should().Be("Successfully registered sale");
        }

        [Fact]
        public void RegisterSale_WithInvalidCPF_ThrowsArgumentException()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "11122233344",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>
        {
            new ItemData { Item_Name = "Item 1" },
            new ItemData { Item_Name = "Item 2" }
        }
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.RegisterSale(salesData);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid CPF");
        }

        [Fact]
        public void RegisterSale_WithCPFContainingSameDigits_ThrowsArgumentException()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "11111111111",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>
        {
            new ItemData { Item_Name = "Item 1" },
            new ItemData { Item_Name = "Item 2" }
        }
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.RegisterSale(salesData);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid CPF");
        }

        [Fact]
        public void RegisterSale_WithNonNumericCPF_ThrowsArgumentException()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "ABC12345678",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>
        {
            new ItemData { Item_Name = "Item 1" },
            new ItemData { Item_Name = "Item 2" }
        }
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.RegisterSale(salesData);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid CPF");
        }

        [Fact]
        public void RegisterSale_WithEmptyItemList_ThrowsArgumentException()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "123456789",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>() // Empty item list
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.RegisterSale(salesData);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("At least one item is required to register a sale");
        }

        [Fact]
        public void RegisterSale_WithEmptyItemName_ThrowsArgumentException()
        {
            // Arrange
            var salesData = new SalesData
            {
                Seller_Data = new SellerData
                {
                    CPF = "12345678909",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    CellPhone = "12345678901"
                },
                Item_Data = new List<ItemData>
            {
                new ItemData { Item_Name = "Item 1" },
                new ItemData { Item_Name = "" } // Empty item name
            }
            };

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.RegisterSale(salesData);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Item name cannot be empty");
        }
        #endregion

        #region GetSaleList

        [Fact]
        public void GetSaleList_WithExistingSales_ReturnsSalesList()
        {
            // Arrange
            var salesList = new List<Sale>
            {
                new Sale { Id = 1, Date = DateTime.UtcNow, Status = SaleStatus.AwaitingPayment },
                new Sale { Id = 2, Date = DateTime.UtcNow, Status = SaleStatus.PaymentApproved }
            };

            _saleRepositoryMock.Setup(x => x.GetSaleList()).Returns(salesList);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.GetSaleList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(salesList.Count);
            result.Should().BeEquivalentTo(salesList);
        }

        [Fact]
        public void GetSaleList_WithNoSales_ThrowsNotFoundException()
        {
            // Arrange
            _saleRepositoryMock.Setup(x => x.GetSaleList()).Returns(new List<Sale>());

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.GetSaleList();

            // Assert
            action.Should().Throw<Exception>().WithMessage("Error fetching sales: No sales found");
        }

        [Fact]
        public void GetSaleList_WithExceptionThrown_ThrowsException()
        {
            // Arrange
            _saleRepositoryMock.Setup(x => x.GetSaleList()).Throws<Exception>();

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.GetSaleList();

            // Assert
            action.Should().Throw<Exception>().WithMessage("Error fetching sales: Exception of type 'System.Exception' was thrown.");
        }

        #endregion

        #region GetSale

        [Fact]
        public void GetSale_WithExistingSale_ReturnsSaleDetails()
        {
            // Arrange
            var saleId = 1;
            var sale = new Sale
            {
                Id = saleId,
                Date = DateTime.UtcNow,
                Status = SaleStatus.AwaitingPayment,
                Seller = new Seller
                {
                    CPF = "123456789",
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    Cell_Phone = "1234567890"
                },
                Items = new List<Item>
                {
                    new Item { Item_Name = "Item 1" },
                    new Item { Item_Name = "Item 2" }
                }
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.GetSale(saleId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(saleId);
            result.Date.Should().Be(sale.Date);
            result.Status.Should().Be(sale.Status);
            result.Seller.Should().NotBeNull();
            result.Seller.CPF.Should().Be(sale.Seller.CPF);
            result.Seller.Name.Should().Be(sale.Seller.Name);
            result.Seller.Email.Should().Be(sale.Seller.Email);
            result.Seller.CellPhone.Should().Be(sale.Seller.Cell_Phone);
            result.Items.Should().HaveCount(sale.Items.Count);
            result.Items.Should().BeEquivalentTo(sale.Items.Select(i => new ItemData { Item_Name = i.Item_Name }));
        }

        [Fact]
        public void GetSale_WithNonExistingSale_ThrowsNotFoundException()
        {
            // Arrange
            var saleId = 1;
            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns((Sale)null);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.GetSale(saleId);

            // Assert
            action.Should().Throw<NotFoundException>().WithMessage($"Sale with ID {saleId} not found");
        }

        #endregion

        #region UpdateSaleStatus

        [Fact]
        public void UpdateSaleStatus_WithValidData_ReturnsStatusChangeResponse()
        {
            // Arrange
            var saleId = 1;
            var newStatus = SaleStatus.PaymentApproved;
            var expectedMessage = "Status changed from AwaitingPayment to PaymentApproved";

            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.AwaitingPayment
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);
            _saleRepositoryMock.Setup(x => x.UpdateSale(sale));

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            result.Should().NotBeNull();
            result.OldStatus.Should().Be(SaleStatus.AwaitingPayment.ToString());
            result.NewStatus.Should().Be(newStatus.ToString());
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void UpdateSaleStatus_FromAwaitingPaymentToPaymentApproved_ReturnsStatusChangeResponse()
        {
            // Arrange
            var saleId = 0;
            var newStatus = SaleStatus.PaymentApproved;
            var expectedMessage = "Status changed from AwaitingPayment to PaymentApproved";

            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.AwaitingPayment
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);
            _saleRepositoryMock.Setup(x => x.UpdateSale(sale));

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            result.Should().NotBeNull();
            result.OldStatus.Should().Be(SaleStatus.AwaitingPayment.ToString());
            result.NewStatus.Should().Be(newStatus.ToString());
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void UpdateSaleStatus_FromAwaitingPaymentToCanceled_ReturnsStatusChangeResponse()
        {
            // Arrange
            var saleId = 0;
            var newStatus = SaleStatus.Canceled;
            var expectedMessage = "Status changed from AwaitingPayment to Canceled";

            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.AwaitingPayment
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);
            _saleRepositoryMock.Setup(x => x.UpdateSale(sale));

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            result.Should().NotBeNull();
            result.OldStatus.Should().Be(SaleStatus.AwaitingPayment.ToString());
            result.NewStatus.Should().Be(newStatus.ToString());
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void UpdateSaleStatus_FromPaymentApprovedToSentToCarrier_ReturnsStatusChangeResponse()
        {
            // Arrange
            var saleId = 2;
            var newStatus = SaleStatus.SentToCarrier;
            var expectedMessage = "Status changed from PaymentApproved to SentToCarrier";

            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.PaymentApproved
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);
            _saleRepositoryMock.Setup(x => x.UpdateSale(sale));

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            result.Should().NotBeNull();
            result.OldStatus.Should().Be(SaleStatus.PaymentApproved.ToString());
            result.NewStatus.Should().Be(newStatus.ToString());
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void UpdateSaleStatus_FromPaymentApprovedToCanceled_ReturnsStatusChangeResponse()
        {
            // Arrange
            var saleId = 2;
            var newStatus = SaleStatus.Canceled;
            var expectedMessage = "Status changed from PaymentApproved to Canceled";

            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.PaymentApproved
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);
            _saleRepositoryMock.Setup(x => x.UpdateSale(sale));

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            result.Should().NotBeNull();
            result.OldStatus.Should().Be(SaleStatus.PaymentApproved.ToString());
            result.NewStatus.Should().Be(newStatus.ToString());
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void UpdateSaleStatus_FromSentToCarrierToDelivered_ReturnsStatusChangeResponse()
        {
            // Arrange
            var saleId = 3;
            var newStatus = SaleStatus.Delivered;
            var expectedMessage = "Status changed from SentToCarrier to Delivered";

            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.SentToCarrier
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);
            _saleRepositoryMock.Setup(x => x.UpdateSale(sale));

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            var result = salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            result.Should().NotBeNull();
            result.OldStatus.Should().Be(SaleStatus.SentToCarrier.ToString());
            result.NewStatus.Should().Be(newStatus.ToString());
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromAwaitingPaymentToSentToCarrier_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 0;
            SaleStatus newStatus = SaleStatus.SentToCarrier;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.AwaitingPayment
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromAwaitingPaymentToDelivered_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 0;
            SaleStatus newStatus = SaleStatus.Delivered;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.AwaitingPayment
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromPaymentApprovedToAwaitingPayment_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 2;
            SaleStatus newStatus = SaleStatus.AwaitingPayment;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.PaymentApproved
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromPaymentApprovedToDelivered_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 2;
            SaleStatus newStatus = SaleStatus.Delivered;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.PaymentApproved
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromSentToCarrierToAwaitingPayment_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 2;
            SaleStatus newStatus = SaleStatus.AwaitingPayment;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.SentToCarrier
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromSentToCarrierToPaymentApproved_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 2;
            SaleStatus newStatus = SaleStatus.PaymentApproved;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.SentToCarrier
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromSentToCarrierToCanceled_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 2;
            SaleStatus newStatus = SaleStatus.Canceled;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.SentToCarrier
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromDeliveredToAwaitingPayment_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 3;
            SaleStatus newStatus = SaleStatus.AwaitingPayment;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.Delivered
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromDeliveredToPaymentApproved_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 3;
            SaleStatus newStatus = SaleStatus.PaymentApproved;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.Delivered
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromDeliveredToSentToCarrier_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 3;
            SaleStatus newStatus = SaleStatus.SentToCarrier;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.Delivered
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithInvalidTransitionFromDeliveredToCanceled_ThrowsArgumentException()
        {
            // Arrange
            int saleId = 3;
            SaleStatus newStatus = SaleStatus.Canceled;
            var sale = new Sale
            {
                Id = saleId,
                Status = SaleStatus.Delivered
            };

            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns(sale);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, newStatus);

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Invalid status transition");
        }

        [Fact]
        public void UpdateSaleStatus_WithNonExistingSale_ThrowsNotFoundException()
        {
            // Arrange
            var saleId = 1;
            _saleRepositoryMock.Setup(x => x.GetSale(saleId)).Returns((Sale)null);

            var salesService = new SaleService(_saleRepositoryMock.Object, _sellerRepositoryMock.Object, _itemRepositoryMock.Object);

            // Act
            Action action = () => salesService.UpdateSaleStatus(saleId, SaleStatus.PaymentApproved);

            // Assert
            action.Should().Throw<NotFoundException>().WithMessage($"Sale with ID {saleId} not found");
        }

        #endregion

    }
}
