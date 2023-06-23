using System.ComponentModel.DataAnnotations;

namespace Application.Models.Seller
{
    public class SellerData
    {
        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CPF must have exactly 11 digits (0-9).")]
        public string? CPF { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]{3,}$", ErrorMessage = "Name must have at least 3 letters (A-Z).")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CellPhone must have exactly 11 digits (0-9).")]
        public string? CellPhone { get; set; }

    }
}
