using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eBot.Commands
{
    public interface IBotCommand
    {
        Task ExecuteAsync(Message message, TelegramBotClient client);
    }
}