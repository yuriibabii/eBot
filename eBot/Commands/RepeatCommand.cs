using System.Threading.Tasks;
using eBot.DbContexts;
using eBot.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class RepeatCommand : Command, IPublicAvailableCommand
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<StudyNewCommand> logger;
        
        public RepeatCommand(IServiceScopeFactory serviceScopeFactory, ILogger<StudyNewCommand> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }
        
        public override string Name => Strings.Commands.Repeat;

        public string HumanReadableDescription => "Shows you a word to repeat.";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var chatId = message.Chat.Id;
            var user = await studyContext.Users.FirstOrDefaultAsync(u => u.Id == chatId);
            if (user == null)
            {
                await botClient.SendTextMessageAsync(chatId, $"Please run {Strings.Commands.Start} first!", ParseMode.Markdown);
                return;
            }

            var rememberElement = user.ElementsInProgress.GetBestToRepeatElement();
            if (rememberElement == null)
            {
                await botClient.SendTextMessageAsync(
                    chatId,
                    $"There is nothing to repeat for now. Please call {Strings.Commands.Study} to study a new word, or try later.",
                    ParseMode.Markdown);
                return;
            }
            
            await botClient.SendTextMessageAsync(chatId, rememberElement.ToString(), ParseMode.Markdown);
            await studyContext.SaveChangesAsync();
        }
    }
}