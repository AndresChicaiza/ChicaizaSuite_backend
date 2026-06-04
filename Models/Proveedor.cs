using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(13)]
        public string Ruc { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string RazonSocial { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(25)]
        public string Telefono { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Estado { get; set; } = "Activo";
    }
}
