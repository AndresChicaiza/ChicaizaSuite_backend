using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Clientes.OrderBy(c => c.RazonSocial).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            cliente.CreadoEn = DateTime.UtcNow;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return Ok(cliente);
        }
    }
}
