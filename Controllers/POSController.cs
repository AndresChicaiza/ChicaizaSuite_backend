using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class POSController : ControllerBase
    {
        private readonly AppDbContext _context;

        public POSController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("cajas")]
        public async Task<IActionResult> GetCajas()
        {
            var cajas = await _context.CajasPOS.ToListAsync();
            return Ok(cajas);
        }

        [HttpPost("cajas/abrir")]
        public async Task<IActionResult> AbrirCaja([FromBody] AbrirCajaDto dto)
        {
            var caja = await _context.CajasPOS.FindAsync(dto.CajaId);
            if (caja == null) return NotFound("Caja no encontrada.");
            
            if (caja.Estado == "Abierta") return BadRequest("La caja ya está abierta.");

            var session = new SesionPOS
            {
                CajaId = caja.Id,
                UsuarioAperturaId = dto.UsuarioId,
                SaldoInicial = dto.SaldoInicial,
                Estado = "Abierta"
            };

            caja.Estado = "Abierta";
            _context.SesionesPOS.Add(session);
            await _context.SaveChangesAsync();

            return Ok(session);
        }

        [HttpPost("cajas/cerrar/{sesionId}")]
        public async Task<IActionResult> CerrarCaja(int sesionId, [FromBody] CerrarCajaDto dto)
        {
            var sesion = await _context.SesionesPOS.Include(s => s.Caja).FirstOrDefaultAsync(s => s.Id == sesionId);
            if (sesion == null) return NotFound("Sesión no encontrada.");
            if (sesion.Estado == "Cerrada") return BadRequest("La sesión ya está cerrada.");

            sesion.FechaCierre = DateTime.UtcNow;
            sesion.SaldoFinalReal = dto.SaldoFinalReal;
            sesion.Estado = "Cerrada";
            
            if (sesion.Caja != null)
                sesion.Caja.Estado = "Cerrada";

            await _context.SaveChangesAsync();
            return Ok(sesion);
        }
    }

    public class AbrirCajaDto
    {
        public int CajaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal SaldoInicial { get; set; }
    }

    public class CerrarCajaDto
    {
        public decimal SaldoFinalReal { get; set; }
    }
}
