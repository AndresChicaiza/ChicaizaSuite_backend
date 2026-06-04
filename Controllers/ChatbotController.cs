using Microsoft.AspNetCore.Mvc;
using ChicaizaSuite.Api.Services;

namespace ChicaizaSuite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly GroqChatbotService _chatbotService;

        public ChatbotController(GroqChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return BadRequest("La consulta no puede estar vacía.");

            var response = await _chatbotService.ResponderConsultaAsync(request.Query);
            return Ok(new { Respuesta = response });
        }
    }

    public class ChatRequest
    {
        public string Query { get; set; } = string.Empty;
    }
}
