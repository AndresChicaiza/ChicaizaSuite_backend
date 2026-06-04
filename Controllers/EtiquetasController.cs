using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Services;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtiquetasController : ControllerBase
    {
        private readonly ZebraPrintService _zebraService;

        public EtiquetasController(ZebraPrintService zebraService)
        {
            _zebraService = zebraService;
        }

        // POST api/etiquetas/imprimir
        [HttpPost("imprimir")]
        public async Task<IActionResult> ImprimirEtiqueta([FromBody] EtiquetaRequest request)
        {
            string zpl = _zebraService.GenerateFashionLabelZpl(
                request.NombreProducto,
                request.Talla,
                request.Color,
                request.CodigoBarras,
                request.Precio
            );

            bool success = await _zebraService.PrintLabelOverIpAsync(
                request.IpImpresora,
                request.Puerto,
                zpl
            );

            if (success)
                return Ok(new { Message = "Etiqueta enviada a la impresora.", ZPL = zpl });

            return StatusCode(500, new { Message = "Error al conectar con la impresora Zebra." });
        }

        // POST api/etiquetas/preview
        [HttpPost("preview")]
        public IActionResult PreviewZpl([FromBody] EtiquetaRequest request)
        {
            string zpl = _zebraService.GenerateFashionLabelZpl(
                request.NombreProducto,
                request.Talla,
                request.Color,
                request.CodigoBarras,
                request.Precio
            );

            return Ok(new { ZPL = zpl });
        }
    }

    public class EtiquetaRequest
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string Talla { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string IpImpresora { get; set; } = "192.168.1.100";
        public int Puerto { get; set; } = 9100;
    }
}
