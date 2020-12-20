using System.Threading.Tasks;
using eBot.Data.Domain;
using eBot.Data.Persistent;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public class StudyNewCommand : BotCommand, IPublicAvailableCommand
    {
        public const string Name = Strings.Commands.Study;

        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<StudyNewCommand> logger;
        
        public StudyNewCommand(IServiceScopeFactory serviceScopeFactory, ILogger<StudyNewCommand> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }
        
        public string HumanReadableDescription => "Shows you a new word to study.";

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var chatId = message.Chat.Id;
            var user = await studyContext
                .Users
                .FirstOrNullAsync(u => u.Id == chatId)
                .MapAsync(UserMapper.Map);
            
            if (user == null)
            {
                await botClient.SendTextMessageAsync(chatId, $"Please run {Strings.Commands.Start} first!", ParseMode.Markdown);
                return;
            }
            
            var newElementId = user.NextElementToStudyId;
            var newVocabularyElementDb = await studyContext.Vocabulary.FindAsync(newElementId);
            if (newVocabularyElementDb == null)
            {
                logger.LogError($"{nameof(newVocabularyElementDb)} is null. New word to study isn't found.");
                await botClient.SendTextMessageAsync(chatId, $"New word to study isn't available. Please try later.", ParseMode.Markdown);
                return;
            }

            var vocabularyElement = newVocabularyElementDb.Map();
            var rememberElement = new RememberElement(vocabularyElement);
            user.ElementsInProgress.Add(rememberElement);
            await botClient.SendTextMessageAsync(chatId, vocabularyElement.ToString(), ParseMode.Markdown);
            await studyContext.SaveChangesAsync();
        }
    }
}