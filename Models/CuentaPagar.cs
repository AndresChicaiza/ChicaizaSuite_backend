using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("CuentasPagar")]
    public class CuentaPagar
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        [ForeignKey("Compra")]
        public int? CompraId { get; set; }
        public Compra? Compra { get; set; }

        [MaxLength(200)]
        public string Concepto { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoPendiente { get; set; }

        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        public DateTime FechaVencimiento { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Pagada, Vencida
    }
}
