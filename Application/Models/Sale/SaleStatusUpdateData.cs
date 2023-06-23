using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Sale
{
    public class SaleStatusUpdateData
    {
        [Required]
        public SaleStatus Status { get; set; }
    }
}
