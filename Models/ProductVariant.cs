using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("ProductoVariantes")]
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Product? Producto { get; set; }

        [MaxLength(50)]
        public string Talla { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Color { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CodigoBarrasEspecifico { get; set; }

        public int Stock { get; set; } = 0;
    }
}
