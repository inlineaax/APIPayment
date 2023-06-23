using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("seller")]
    public class Seller
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("cpf")]
        public string? CPF { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("cell_phone")]
        public string? Cell_Phone { get; set; }

        [Column("saleId")]
        public int SaleId { get; set; }

        public Sale? Sale { get; set; }
    }
}
