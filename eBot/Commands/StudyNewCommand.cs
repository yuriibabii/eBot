using System.Threading.Tasks;
using eBot.Data.Domain;
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
    public class StudyNewCommand : BotCommand
    {
        public const string Name = Strings.Commands.Study;
        public const string HumanReadableDescription = "Shows you a new word to study.";
        
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<StudyNewCommand> logger;
        
        public StudyNewCommand(IServiceScopeFactory serviceScopeFactory, ILogger<StudyNewCommand> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            await base.ExecuteAsync(message, botClient);
            
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var userDb = await studyContext
                .Users
                .FirstOrNullAsync(u => u.Id == ChatId);
            
            if (userDb == null)
            {
                await botClient.SendTextMessageAsync(ChatId, $"Please run {Strings.Commands.Start} first!", ParseMode.Markdown);
                return;
            }

            var user = userDb.Map(studyContext);
            user.LastCommand = this;
            
            var newElementId = user.NextElementToStudyId;
            var newVocabularyElementDb = await studyContext.Vocabulary.FindAsync(newElementId);
            if (newVocabularyElementDb == null)
            {
                logger.LogError($"{nameof(newVocabularyElementDb)} is null. New word to study isn't found.");
                await botClient.SendTextMessageAsync(ChatId, $"New word to study isn't available. Please try later.", ParseMode.Markdown);
                return;
            }

            var vocabularyElement = newVocabularyElementDb.Map();
            var rememberElement = new RememberElement(vocabularyElement);
            user.ElementsInProgress.Add(rememberElement);
            await botClient.SendTextMessageAsync(ChatId, vocabularyElement.ToString(), ParseMode.Markdown);
            await studyContext.SaveChangesAsync();
        }
    }
}