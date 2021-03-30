using System.Threading.Tasks;
using eBot.Data.Domain;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Mappers;
using eBot.Repositories;
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

        private readonly ILogger<StudyNewCommand> logger;
        private readonly IUserRepository userRepository;

        public StudyNewCommand(IServiceScopeFactory serviceScopeFactory, ILogger<StudyNewCommand> logger)
            : base(serviceScopeFactory)
        {
            this.logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            await base.ExecuteAsync(message, botClient);

            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var user = UserDb?.Map(studyContext);
            if (user == null)
            {
                return;
            }

            user.LastCommand = this;
            await StudyWordFromVocabularyAsync(botClient, user);
        }

        private async Task StudyWordFromVocabularyAsync(TelegramBotClient botClient, Data.Domain.User user)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var newElementId = user.NextElementToStudyId;
            var newVocabularyElementDb = await studyContext.Vocabulary.FindAsync(newElementId);
            if (newVocabularyElementDb == null)
            {
                logger.LogError($"{nameof(newVocabularyElementDb)} is null. New word to study isn't found.");
                await botClient.SendTextMessageAsync(ChatId, $"New word to study isn't available. Please try later.", ParseMode.Markdown);
                return;
            }

            var vocabularyElement = newVocabularyElementDb.Map();
            var rememberElement = new VocabStudyElement(vocabularyElement);
            user.ElementsInProgress.Add(rememberElement);
            await botClient.SendTextMessageAsync(ChatId, vocabularyElement.ToString(), ParseMode.Markdown);
            await studyContext.SaveChangesAsync();
        }
    }
}