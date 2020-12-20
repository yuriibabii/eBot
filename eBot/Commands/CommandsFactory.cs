using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public static class CommandsFactory
    {
        public static IBotCommand ProduceNewForUpdate(Update update, IServiceProvider serviceProvider)
        {
            if (update.Type != UpdateType.Message)
            {
                return new HelpCommand();
            }
            
            return update.Message.Text switch
            {
                var m when m.Contains(StartCommand.Name) => CreateStartCommand(serviceProvider),
                // var m when m.Contains(HelpCommand.Name) => typeof(HelpCommand),
                // var m when m.Contains(RepeatCommand.Name) => typeof(RepeatCommand),
                // var m when m.Contains(StudyNewCommand.Name) => typeof(StudyNewCommand),
                // _ => typeof(HelpCommand)
            };
        }

        private static StartCommand CreateStartCommand(IServiceProvider serviceProvider)
        {
            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
            return new StartCommand(serviceScopeFactory);
        }
    }
}