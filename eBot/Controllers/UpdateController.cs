using System;
using System.Threading.Tasks;
using eBot.Commands;
using eBot.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Controllers
{
    [Route("/")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        [HttpPost]
        public async Task<OkResult> Post(
            [FromBody] Update? update,
            [FromServices] IServiceProvider serviceProvider)
        {
            try
            {
                if (update == null)
                {
                    return Ok();
                }

                var logger = serviceProvider.Resolve<ILogger<UpdateController>>();
                logger.Log(LogLevel.Information, "callback query --- " + update.CallbackQuery?.Message?.Text);
                logger.Log(LogLevel.Information, "inline query ---" + update.InlineQuery?.Query);
                logger.Log(LogLevel.Information, "inline result ---" + update.ChosenInlineResult?.Query);
                logger.Log(LogLevel.Information, "message text ---" + update.Message?.Text);
                logger.Log(LogLevel.Information, "message type ---" + Enum.GetName(typeof(MessageType), update.Message?.Type));

                var botClient = await Bot.GetBotClientAsync();
                var commandToExecute = CommandsFactory.ProduceNewForUpdate(update, serviceProvider);
                await commandToExecute.ExecuteAsync(update.Message, botClient);
            }
            catch (Exception e)
            {
                var logger = serviceProvider.Resolve<ILogger<UpdateController>>();
                logger.Log(LogLevel.Critical, e.ToString());
            }

            return Ok();
        }
    }
}