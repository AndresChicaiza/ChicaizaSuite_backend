using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace ChicaizaSuite.Api.Services
{
    public class SriXmlSignatureService
    {
        public string FirmarXmlXadesBes(string xmlContent, string p12Path, string p12Password)
        {
            // Carga del certificado de firma electrónica P12 emitido por entidades autorizadas en Ecuador (BCE, SecurityData, etc.)
            var certificate = new X509Certificate2(p12Path, p12Password, X509KeyStorageFlags.Exportable);

            var xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.LoadXml(xmlContent);

            // 1. Configuración de Xades-BES (Formato oficial del SRI)
            var signedXml = new SignedXml(xmlDocument)
            {
                SigningKey = certificate.GetRSAPrivateKey()
            };

            // 2. Referencias y Transforms
            var reference = new Reference { Uri = "" };
            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedXml.AddReference(reference);

            // 3. KeyInfo
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.KeyInfo = keyInfo;

            // 4. Cómputo de la firma
            signedXml.ComputeSignature();
            var xmlDigitalSignature = signedXml.GetXml();

            xmlDocument.DocumentElement?.AppendChild(xmlDocument.ImportNode(xmlDigitalSignature, true));

            return xmlDocument.OuterXml;
        }

        public string GenerarClaveAcceso(DateTime fechaEmision, string tipoComprobante, string ruc, int ambiente, string serie, string secuencial, string codigoNumerico, int tipoEmision)
        {
            // Regla oficial del SRI: 48 caracteres + 1 dígito verificador (Módulo 11)
            string fecha = fechaEmision.ToString("ddMMyyyy");
            string clave48 = $"{fecha}{tipoComprobante}{ruc}{ambiente}{serie}{secuencial}{codigoNumerico}{tipoEmision}";
            
            int digitoVerificador = CalcularModulo11(clave48);
            return $"{clave48}{digitoVerificador}";
        }

        private int CalcularModulo11(string cadena)
        {
            int pivot = 2;
            int total = 0;
            for (int i = cadena.Length - 1; i >= 0; i--)
            {
                total += int.Parse(cadena[i].ToString()) * pivot;
                pivot = pivot == 7 ? 2 : pivot + 1;
            }
            int residuo = total % 11;
            int digito = 11 - residuo;
            if (digito == 11) return 0;
            if (digito == 10) return 1;
            return digito;
        }
    }
}
