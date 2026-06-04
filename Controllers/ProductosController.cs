using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/productos
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? buscar, [FromQuery] string? categoria)
        {
            var query = _context.Productos.Include(p => p.Variantes).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
                query = query.Where(p => p.Nombre.Contains(buscar) || p.Codigo.Contains(buscar));

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria == categoria);

            var productos = await query.OrderBy(p => p.Nombre).ToListAsync();
            return Ok(productos);
        }

        // GET api/productos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Variantes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null) return NotFound();
            return Ok(producto);
        }

        // POST api/productos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product producto)
        {
            producto.CreadoEn = DateTime.UtcNow;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        // PUT api/productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product productoUpdate)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            producto.Nombre = productoUpdate.Nombre;
            producto.Codigo = productoUpdate.Codigo;
            producto.CodigoBarras = productoUpdate.CodigoBarras;
            producto.Descripcion = productoUpdate.Descripcion;
            producto.Categoria = productoUpdate.Categoria;
            producto.PrecioVenta = productoUpdate.PrecioVenta;
            producto.PrecioCosto = productoUpdate.PrecioCosto;
            producto.IvaTipo = productoUpdate.IvaTipo;
            producto.IvaIncluido = productoUpdate.IvaIncluido;
            producto.Estado = productoUpdate.Estado;

            await _context.SaveChangesAsync();
            return Ok(producto);
        }

        // DELETE api/productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
