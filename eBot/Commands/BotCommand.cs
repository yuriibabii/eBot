using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using eBot.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using eBot.Extensions;
using Telegram.Bot.Types.Enums;
using eBot.Data.Persistent;

namespace eBot.Commands
{
    public abstract class BotCommand : IBotCommand
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;

        public BotCommand(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected long ChatId { get; private set; }

        protected UserDb? UserDb { get; private set; }

        public virtual async Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();

            ChatId = message.Chat.Id;
            UserDb = await studyContext
                .Users
                .FirstOrNullAsync(u => u.Id == ChatId);

            if (UserDb == null)
            {
                await client.SendTextMessageAsync(ChatId, $"Please run {Strings.Commands.Start} first!", ParseMode.Markdown);
                return;
            }
        }
    }
}