using System.ComponentModel.DataAnnotations;

namespace Application.Models.Item
{
    public class ItemData
    {
        [Required]
        public string? Item_Name { get; set; }
    }
}
