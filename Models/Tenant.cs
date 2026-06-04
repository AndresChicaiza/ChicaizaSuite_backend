using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Tenants")]
    public class Tenant
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Ruc { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? LogoUrl { get; set; }

        [MaxLength(10)]
        public string Estado { get; set; } = "Activo";

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
