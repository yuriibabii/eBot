using System.Threading.Tasks;
using Telegram.Bot;

namespace eBot
{
    public static class Bot
    {
        private static TelegramBotClient? botClient;

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }
            
            botClient = new TelegramBotClient(AppSettings.TelegramBotToken);
            await botClient.SetWebhookAsync(AppSettings.NGrokUrl);
            return botClient;
        }
    }
}