using System.Net.Sockets;
using System.Text;

namespace ChicaizaSuite.Api.Services
{
    public class ZebraPrintService
    {
        private readonly ILogger<ZebraPrintService> _logger;

        public ZebraPrintService(ILogger<ZebraPrintService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> PrintLabelOverIpAsync(string ipAddress, int port, string zplCommand)
        {
            try
            {
                using var client = new TcpClient();
                // Zebra printers typically use port 9100 for raw TCP socket printing
                await client.ConnectAsync(ipAddress, port);

                using var stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(zplCommand);

                await stream.WriteAsync(data, 0, data.Length);
                await stream.FlushAsync();
                
                _logger.LogInformation($"Etiqueta enviada exitosamente a la impresora Zebra en {ipAddress}:{port}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al imprimir etiqueta Zebra en IP {ipAddress}:{port}");
                return false;
            }
        }

        public string GenerateFashionLabelZpl(string productName, string size, string color, string barcode, decimal price)
        {
            // Etiqueta estándar 50x25mm para ropa
            // ^XA : Inicio
            // ^FO : Field Origin
            // ^A0 : Font
            // ^BC : Barcode 128
            // ^XZ : Fin
            
            var sb = new StringBuilder();
            sb.AppendLine("^XA");
            
            // Nombre del producto (arriba)
            sb.AppendLine($"^FO20,20^A0N,25,25^FD{productName}^FS");
            
            // Variante: Talla y Color
            sb.AppendLine($"^FO20,50^A0N,20,20^FDTalla: {size} | Color: {color}^FS");
            
            // Precio destacado
            sb.AppendLine($"^FO250,20^A0N,30,30^FD${price:0.00}^FS");
            
            // Código de barras (Code 128)
            sb.AppendLine($"^FO20,80^BCN,60,Y,N,N^FD{barcode}^FS");
            
            sb.AppendLine("^XZ");

            return sb.ToString();
        }
    }
}
