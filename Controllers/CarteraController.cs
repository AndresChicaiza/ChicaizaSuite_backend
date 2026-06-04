using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarteraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarteraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("cxc")]
        public async Task<IActionResult> GetCuentasPorCobrar()
        {
            return Ok(await _context.CuentasCobrar.Include(c => c.Cliente).ToListAsync());
        }

        [HttpGet("cxp")]
        public async Task<IActionResult> GetCuentasPorPagar()
        {
            return Ok(await _context.CuentasPagar.Include(c => c.Proveedor).ToListAsync());
        }
    }
}
