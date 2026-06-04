using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;
using ChicaizaSuite.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SriXmlSignatureService _sriService;

        public FacturasController(AppDbContext context, SriXmlSignatureService sriService)
        {
            _context = context;
            _sriService = sriService;
        }

        // GET api/facturas
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? estado, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            var query = _context.Facturas.Include(f => f.Detalles).AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(f => f.EstadoSRI == estado);

            if (desde.HasValue)
                query = query.Where(f => f.FechaEmision >= desde.Value);

            if (hasta.HasValue)
                query = query.Where(f => f.FechaEmision <= hasta.Value);

            var facturas = await query.OrderByDescending(f => f.FechaEmision).ToListAsync();
            return Ok(facturas);
        }

        // GET api/facturas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null) return NotFound();
            return Ok(factura);
        }

        // POST api/facturas
        [HttpPost]
        public async Task<IActionResult> Emitir([FromBody] Factura factura)
        {
            factura.FechaEmision = DateTime.UtcNow;
            factura.EstadoSRI = "Creada";

            // Generar Clave de Acceso SRI (49 dígitos)
            factura.ClaveAccesoSRI = _sriService.GenerarClaveAcceso(
                factura.FechaEmision,
                "01",                          // 01 = Factura
                "0000000000001",               // RUC del emisor (se configura en appsettings)
                factura.Ambiente,
                "001001",                      // Serie: Establecimiento + Punto de Emisión
                factura.NumeroComprobante.PadLeft(9, '0'),
                new Random().Next(10000000, 99999999).ToString(),
                factura.TipoEmision
            );

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = factura.Id }, factura);
        }

        // POST api/facturas/5/anular
        [HttpPost("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null) return NotFound();

            factura.EstadoSRI = "Anulada";
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Factura anulada correctamente." });
        }
    }
}
