using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMPOLAssistant.API.Models;


namespace IMPOLAssistant.API.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpPost("send")]
        public async IAsyncEnumerable<Message> SendMessage([FromBody] Message userMessage)
        {
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
                Content = "Odgovor: " + userMessage.Content,
                Timestamp = DateTime.Now
            };
        }
    }
}
