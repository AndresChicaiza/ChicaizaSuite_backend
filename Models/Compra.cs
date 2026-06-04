using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Compras")]
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        [Required, MaxLength(50)]
        public string NumeroFacturaProveedor { get; set; } = string.Empty;

        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "Recibida"; // Recibida, Anulada
    }
}
