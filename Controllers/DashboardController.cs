using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChicaizaSuite.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Uncomment later to protect the endpoint
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            var totalClientes = await _context.Clientes.CountAsync();
            var totalProductos = await _context.Productos.CountAsync();
            var totalFacturas = await _context.Facturas.CountAsync();
            var ingresosTotales = await _context.Facturas.SumAsync(f => (decimal?)f.ValorTotal) ?? 0;

            var kpis = new
            {
                TotalClientes = totalClientes,
                TotalProductos = totalProductos,
                TotalFacturas = totalFacturas,
                IngresosTotales = ingresosTotales
            };

            return Ok(kpis);
        }
    }
}
