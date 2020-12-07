using System.Threading.Tasks;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class StudyNewCommand : Command, IPublicAvailableCommand
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<StudyNewCommand> logger;
        
        public StudyNewCommand(IServiceScopeFactory serviceScopeFactory, ILogger<StudyNewCommand> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }
        
        public override string Name => Strings.Commands.Study;

        public string HumanReadableDescription => "Shows you a new word to study.";

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

            var newElementId = user.NextElementToStudyId;
            var newElement = await studyContext.Vocabulary.FindAsync(newElementId);
            if (newElement == null)
            {
                logger.LogError($"{nameof(newElement)} is null. New word to study isn't found.");
                await botClient.SendTextMessageAsync(chatId, $"New word to study isn't available. Please try later.", ParseMode.Markdown);
                return;
            }
            
            var rememberElement = new RememberElement(newElement);
            user.ElementsInProgress.Add(rememberElement);
            await botClient.SendTextMessageAsync(chatId, newElement.ToString(), ParseMode.Markdown);
            await studyContext.SaveChangesAsync();
        }
    }
}