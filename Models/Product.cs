using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Productos")]
    public class Product : IMustHaveTenant
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        [Required, MaxLength(30)]
        public string Codigo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CodigoBarras { get; set; }

        [Required, MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [MaxLength(100)]
        public string Categoria { get; set; } = string.Empty;

        public string? ImagenUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCosto { get; set; }

        [MaxLength(10)]
        public string IvaTipo { get; set; } = "15"; // SRI Ecuador now has 15% IVA usually

        public bool IvaIncluido { get; set; } = false;

        public int StockGeneral { get; set; } = 0;

        [MaxLength(10)]
        public string Estado { get; set; } = "Activo";

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        
        public ICollection<ProductVariant> Variantes { get; set; } = new List<ProductVariant>();
    }
}
