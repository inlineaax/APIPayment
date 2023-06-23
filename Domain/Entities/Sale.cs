using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("sale")]
    public class Sale
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("status")]
        public SaleStatus Status { get; set; }

        public Seller? Seller { get; set; }
        public List<Item>? Items { get; set; }

    }
}
