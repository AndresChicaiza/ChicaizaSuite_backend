using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Clientes")]
    public class Cliente : IMustHaveTenant
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        [Required, MaxLength(13)]
        public string Identificacion { get; set; } = string.Empty; // Cédula (10) o RUC (13)

        [Required, MaxLength(250)]
        public string RazonSocial { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(25)]
        public string Telefono { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Ciudad { get; set; } = "Quito";

        [MaxLength(10)]
        public string Estado { get; set; } = "Activo";

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
