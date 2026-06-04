using ChicaizaSuite.Api.Data;
using ChicaizaSuite.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ChicaizaSuite.Api.Services
{
    public class GroqChatbotService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GroqChatbotService(AppDbContext context, IConfiguration config, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _apiKey = config["GROQ_API_KEY"] ?? string.Empty;
        }

        public async Task<string> ResponderConsultaAsync(string queryText)
        {
            if (string.IsNullOrEmpty(_apiKey)) return "⚠️ La API Key de Groq no está configurada en .NET.";

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = "Eres el cerebro de un ERP. El usuario pregunta sobre su negocio." },
                    new { role = "user", content = queryText }
                },
                temperature = 0.3
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content; // Se debe parsear el JSON de respuesta. Simplificado por ahora.
            }

            return "Ocurrió un error al conectar con Groq desde C#.";
        }
    }
}
