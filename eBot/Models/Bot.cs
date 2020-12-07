using System.Collections.Generic;
using System.Threading.Tasks;
using eBot.Commands;
using Telegram.Bot;

namespace eBot.Models
{
    public static class Bot
    {
        private static TelegramBotClient? botClient;
        private static List<Command> commandsList = null!;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<Command>
            {
                new StartCommand(),
                new StudyNewCommand(),
                new RepeatCommand(),
                new HelpCommand()
            };

            botClient = new TelegramBotClient(AppSettings.TelegramBotToken);
            await botClient.SetWebhookAsync(AppSettings.NGrokUrl);
            return botClient;
        }
    }
}