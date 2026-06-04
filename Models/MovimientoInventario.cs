using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("MovimientosInventario")]
    public class MovimientoInventario
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Product? Producto { get; set; }

        [ForeignKey("Variante")]
        public int? VarianteId { get; set; }
        public ProductVariant? Variante { get; set; }

        [ForeignKey("Bodega")]
        public int BodegaId { get; set; }
        public Bodega? Bodega { get; set; }

        [MaxLength(20)]
        public string TipoMovimiento { get; set; } = string.Empty; // ENTRADA, SALIDA, AJUSTE, TRASLADO

        public int Cantidad { get; set; } // Puede ser negativo si es salida

        [MaxLength(200)]
        public string Referencia { get; set; } = string.Empty; // Ej: "Factura 001-0002", "Compra 554"

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
