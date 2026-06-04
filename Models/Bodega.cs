using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("Bodegas")]
    public class Bodega
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // ej: "Matriz Quito", "Sucursal Guayaquil"

        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Estado { get; set; } = "Activa";
    }
}
