using System.Threading.Tasks;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class RepeatCommand : BotCommand
    {
        public const string HumanReadableDescription = "Shows you a word to repeat.";
        public const string Name = Strings.Commands.Repeat;

        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<RepeatCommand> logger;
        
        public RepeatCommand(IServiceScopeFactory serviceScopeFactory, ILogger<RepeatCommand> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }
        
        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            await base.ExecuteAsync(message, botClient);
            
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var user = await studyContext
                .Users
                .FirstOrNullAsync(u => u.Id == ChatId)
                .MapAsync(currentUser => currentUser.Map(studyContext));
            
            if (user == null)
            {
                logger.LogError($"{nameof(user)} is null. Current user isn't found.");
                await botClient.SendTextMessageAsync(ChatId, $"Please run {Strings.Commands.Start} first!", ParseMode.Markdown);
                return;
            }

            user.LastCommand = this;
            
            var rememberElement = user.ElementsInProgress.GetBestToRepeatElement();
            if (rememberElement == null)
            {
                logger.LogWarning($"{nameof(rememberElement)} is null. User have nothing to repeat.");
                await botClient.SendTextMessageAsync(
                    ChatId,
                    $"There is nothing to repeat for now. Please click {Strings.Commands.Study} to study a new word, or try later.",
                    ParseMode.Markdown);
                return;
            }
            
            await botClient.SendTextMessageAsync(ChatId, rememberElement.ToString(), ParseMode.Markdown);
            await studyContext.SaveChangesAsync();
        }
    }
}