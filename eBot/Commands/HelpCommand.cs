using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class HelpCommand : Command
    {
        public override string Name => Strings.Commands.Help;

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            var messageBuilder = new StringBuilder("Commands which you would like to use:");

            var publicAvailableCommands = Bot.Commands.OfType<IPublicAvailableCommand>();
            foreach (var command in publicAvailableCommands)
            {
                messageBuilder.AppendLine($"{command.Name} - {command.HumanReadableDescription}");
            }
            
            await botClient.SendTextMessageAsync(chatId, messageBuilder.ToString(), ParseMode.Markdown);
        }
    }
}