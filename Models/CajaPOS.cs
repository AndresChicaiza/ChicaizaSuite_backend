using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicaizaSuite.Api.Models
{
    [Table("CajasPOS")]
    public class CajaPOS : IMustHaveTenant
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Estado { get; set; } = "Cerrada"; // Abierta, Cerrada

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        public ICollection<SesionPOS> Sesiones { get; set; } = new List<SesionPOS>();
    }

    [Table("SesionesPOS")]
    public class SesionPOS : IMustHaveTenant
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        [ForeignKey("Caja")]
        public int CajaId { get; set; }
        public CajaPOS? Caja { get; set; }

        [ForeignKey("UsuarioApertura")]
        public int UsuarioAperturaId { get; set; }
        public User? UsuarioApertura { get; set; }

        public DateTime FechaApertura { get; set; } = DateTime.UtcNow;
        public DateTime? FechaCierre { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoFinalEsperado { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoFinalReal { get; set; } = 0;

        [MaxLength(10)]
        public string Estado { get; set; } = "Abierta"; // Abierta, Cerrada
    }
}
