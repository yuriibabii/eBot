using System.Threading.Tasks;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = eBot.Data.Domain.User;

namespace eBot.Commands
{
    public class StartCommand : BotCommand
    {
        public const string Name = Strings.Commands.Start;

        private readonly IServiceScopeFactory serviceScopeFactory;

        public StartCommand(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await ShowUserSomeInformationAsync(botClient, chatId);
            await TrySaveUserAsync(chatId);
        }

        private async Task TrySaveUserAsync(long chatId)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var currentUser = await studyContext.Users.FindAsync(chatId);
            if (currentUser == null)
            {
                var user = new User(chatId);
                var userDb = user.Map();
                await studyContext.Users.AddAsync(userDb);
                await studyContext.SaveChangesAsync();
            }
        }

        private async Task ShowUserSomeInformationAsync(TelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId,
                @"Hello! We are here about to start learning Essential English Vocabulary,
which consist from 3600 words. These words are sorted by difficulty from easy to hard.", ParseMode.Markdown);
            
            await botClient.SendTextMessageAsync(chatId,
                $"To study a new word, press {Strings.Commands.Study}.", ParseMode.Markdown);
            
            await botClient.SendTextMessageAsync(chatId,
                $"To repeat a word that you studied, press {Strings.Commands.Repeat}.", ParseMode.Markdown);
            
            await botClient.SendTextMessageAsync(chatId,
                $"If you need some help, press {Strings.Commands.Help}.", ParseMode.Markdown);
        }
    }
}