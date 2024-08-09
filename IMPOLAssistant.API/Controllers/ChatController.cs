using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMPOLAssistant.API.Models;
using IMPOLAssistant.SemanticKernel;


namespace IMPOLAssistant.API.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ISemanticKernelService semanticKernel;

        public ChatController(ISemanticKernelService semanticKernel)
        {
            this.semanticKernel = semanticKernel;
        }

        [HttpPost("send")]
        public async IAsyncEnumerable<Message> SendMessage([FromBody] Message userMessage)
        {
            var odgovor = await this.semanticKernel.ProcessUserQueryAsync(userMessage.Content);
            // Emitovanje korisničke poruke
            yield return new Message
            {
                Content = userMessage.Content,
                Timestamp = DateTime.Now
            };

            // Simulacija odgovora od strane asistenta
            await Task.Delay(1000); // Simulacija kašnjenja
            yield return new Message
            {
                Content = "Odgovor: " + odgovor,
                Timestamp = DateTime.Now
            };
        }
    }
}
