using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("CuentasCobrar")]
    public class CuentaCobrar
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [ForeignKey("Factura")]
        public int? FacturaId { get; set; }
        public Factura? Factura { get; set; }

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
