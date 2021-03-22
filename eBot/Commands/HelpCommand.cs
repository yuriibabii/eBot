using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class HelpCommand : BotCommand
    {
        public const string Name = Strings.Commands.Help;

        public HelpCommand(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {

        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            await base.ExecuteAsync(message, botClient);

            var messageBuilder = new StringBuilder("Commands which you can use:\n");

            var publicCommandsStrings = CommandsFactory.GetPublicCommandsStrings();
            foreach (var (name, humanReadableDescription) in publicCommandsStrings)
            {
                messageBuilder.AppendLine($"{name} - {humanReadableDescription}");
            }

            await botClient.SendTextMessageAsync(ChatId, messageBuilder.ToString(), ParseMode.Markdown);
        }
    }
}