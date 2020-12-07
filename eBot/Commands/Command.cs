using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }

        public virtual Task Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var 
        }

        public virtual bool Contains(Message message)
        {
            return message.Type == MessageType.Text && message.Text.Contains(Name);
        }
    }
}