using System.Threading.Tasks;
using eBot.Models;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace eBot.Controllers
{
    [Route("/")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        [HttpPost]
        public async Task<OkResult> Post([FromBody]Update? update)
        {
            if (update == null)
            {
                return Ok();
            }

            var message = update.Message;
            var botClient = await Bot.GetBotClientAsync();

            var isCommandExecuted = false;
            foreach (var command in Bot.Commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient);
                    isCommandExecuted = true;
                    break;
                }
            }

            if (isCommandExecuted == false)
            {
                
            }

            return Ok();
        }
    }
}