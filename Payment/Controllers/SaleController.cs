using Application.IService;
using Application.Models.Sale;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiPayment.Controllers
{
    [Route("api/sale")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService) => _saleService = saleService;

        /// <summary>
        /// Register a sale
        /// </summary>
        /// <remarks>
        /// Sale must have seller information and at least one item! \
        /// \
        /// About Seller: \
        /// CPF must be 11 numeric characters with no space only \
        /// Name must contain only letters and at least 3 characters \
        /// Email must contain a valid address \
        /// CellPhone must have 11 numeric characters</remarks>
        [HttpPost]
        public ActionResult RegisterSale([FromBody] SalesData sale)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = _saleService.RegisterSale(sale);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering the sale.");
            }
        }

        /// <summary>
        /// Gets all registered sales
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Sale>> GetSaleList()
        {
            try
            {
                var sales = _saleService.GetSaleList();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a sale by Id
        /// </summary>
        /// <remarks>Search by sale Id. To find out a sales Id, first gets for all registered sales!</remarks>
        [HttpGet("{id}")]
        public ActionResult GetSale(int id)
        {
            try
            {
                var sale = _saleService.GetSale(id);
                if (sale == null)
                {
                    return NotFound();
                }
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update a sale Status
        /// </summary>
        /// <remarks>
        /// 0 = Awaiting Payment \
        /// 1 = Payment Approved \
        /// 2 = Sent To Carrier \
        /// 3 = Delivered \
        /// 4 = Canceled
        /// 
        /// The status update must allow only the following transitions: \
        /// \
        /// From: Awaiting Payment to: PaymentApproved \
        /// From: Awaiting Payment to: Canceled \
        /// From: PaymentApproved to: Sent To Carrier \
        /// From: PaymentApproved to: Canceled \
        /// From: Sent To Carrier to: Delivered</remarks> 
        [HttpPatch("{id}/status")]
        public ActionResult UpdateSaleStatus(int id, [FromBody] SaleStatusUpdateData statusData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var newStatus = statusData.Status;
                var result = _saleService.UpdateSaleStatus(id, newStatus);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
