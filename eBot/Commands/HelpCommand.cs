using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class HelpCommand : BotCommand
    {
        public const string Name = Strings.Commands.Help;
        
        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            var messageBuilder = new StringBuilder("Commands which you would like to use:");

            // var publicAvailableCommands = Bot.Commands.OfType<IPublicAvailableCommand>();
            // foreach (var command in publicAvailableCommands)
            // {
            //     messageBuilder.AppendLine($"{command.Name} - {command.HumanReadableDescription}");
            // }
            //
            // await botClient.SendTextMessageAsync(ChatId, messageBuilder.ToString(), ParseMode.Markdown);
        }
    }
}