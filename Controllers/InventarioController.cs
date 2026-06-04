using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("kardex/{productoId}")]
        public async Task<IActionResult> GetKardex(int productoId)
        {
            var movimientos = await _context.MovimientosInventario
                .Include(m => m.Bodega)
                .Where(m => m.ProductoId == productoId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            return Ok(movimientos);
        }

        [HttpPost("ajuste")]
        public async Task<IActionResult> RegistrarAjuste([FromBody] MovimientoInventario ajuste)
        {
            ajuste.Fecha = DateTime.UtcNow;
            ajuste.TipoMovimiento = "AJUSTE";

            var producto = await _context.Productos.FindAsync(ajuste.ProductoId);
            if (producto != null)
            {
                producto.StockGeneral += ajuste.Cantidad;
                _context.MovimientosInventario.Add(ajuste);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Ajuste registrado", StockActual = producto.StockGeneral });
            }

            return NotFound();
        }
    }
}
