using System;
using System.Threading.Tasks;
using eBot.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eBot.Commands
{
    public class StartCommand : BotCommand
    {
        public const string Name = Strings.Commands.Start;
        private readonly IUserRepository userRepository;

        public StartCommand(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            await base.ExecuteAsync(message, botClient);

            var user = await userRepository.GetUserAsync(ChatId);
            if (user == null)
            {
                await ShowUserSomeInformationAsync(botClient, ChatId);
                await userRepository.SaveNewUserAsync(ChatId);
            }
            else
            {
                //TODO: Rethink what we should do if bot was restarted
                await botClient.SendTextMessageAsync(ChatId,
                    $"You can't start {AppSettings.Name} more than once. Use {HelpCommand.Name} to get more info.", ParseMode.Markdown);
            }
        }

        private async Task ShowUserSomeInformationAsync(TelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId,
                $"Hello! {AppSettings.Name} will help you to study English! To learn new words you will use Spaced Repetition technique. There are two options how to study:" + Environment.NewLine +
                "1. Add your own words and phrases." + Environment.NewLine +
                "2. Use Essential English Vocabulary which consist from 3600 words. They sorted by difficulty from easy to hard.",
                ParseMode.Markdown, replyMarkup: new ReplyKeyboardMarkup(new[] {
                    new[]
                    {
                        new KeyboardButton($"📖 {Strings.Commands.StudyName}"),
                        new KeyboardButton($"🚀 {Strings.Commands.MyProgress}"),
                    },
                    new[]
                    {
                        new KeyboardButton($"⚙️ {Strings.Commands.SettingsName}"),
                        new KeyboardButton($"❓ {Strings.Commands.HelpName}")
                    }
                }));


            await Task.Delay(Constants.MessageSendDelay);

            await botClient.SendTextMessageAsync(chatId,
                $"To study a new word, send {Strings.Commands.Study} or press the button on the menu.", ParseMode.Markdown);

            await Task.Delay(Constants.MessageSendDelay);

            await botClient.SendTextMessageAsync(chatId,
                $"If you need some help, send {Strings.Commands.Help} or press the button on the menu.", ParseMode.Markdown);
        }
    }
}