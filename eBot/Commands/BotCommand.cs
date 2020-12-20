using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eBot.Commands
{
    public abstract class BotCommand : IBotCommand
    {
        protected long ChatId { get; private set; }

        public virtual Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            ChatId = message.Chat.Id;
            return Task.CompletedTask;
        }
    }
}