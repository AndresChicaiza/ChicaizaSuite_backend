using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Facturas")]
    public class Factura : IMustHaveTenant
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        [Required, MaxLength(15)]
        public string NumeroComprobante { get; set; } = string.Empty; // ej: 001-001-000000001

        [MaxLength(49)]
        public string ClaveAccesoSRI { get; set; } = string.Empty;

        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

        public int Ambiente { get; set; } = 1; // 1=Pruebas, 2=Produccion

        public int TipoEmision { get; set; } = 1; // 1=Normal

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSinImpuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalImpuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [MaxLength(20)]
        public string EstadoSRI { get; set; } = "Creada"; // Autorizada, Rechazada, Pendiente

        [MaxLength(100)]
        public string ClienteIdentificacion { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string ClienteRazonSocial { get; set; } = string.Empty;

        public ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }

    [Table("FacturaDetalles")]
    public class FacturaDetalle
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Factura")]
        public int FacturaId { get; set; }
        public Factura? Factura { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Product? Producto { get; set; }

        [ForeignKey("Variante")]
        public int? VarianteId { get; set; }
        public ProductVariant? Variante { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
    }
}
