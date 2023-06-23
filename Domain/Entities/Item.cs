using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("item")]
    public class Item
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("saleId")]
        public int SaleId { get; set; }

        [Column("item_name")]
        public string? Item_Name { get; set; }

        public Sale? Sale { get; set; }
    }
}
