using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eBot.Commands
{
    public static class CommandsFactory
    {
        public static IEnumerable<(string Name, string HumanReadableDescription)> GetPublicCommandsStrings()
        {
            yield return (StudyNewCommand.Name, StudyNewCommand.HumanReadableDescription);
            //yield return (RepeatCommand.Name, RepeatCommand.HumanReadableDescription);
        }

        public static IBotCommand? DeserializeCommandByName(this string serializedCommand, string commandTypeName)
        {
            return commandTypeName switch
            {
                var name when nameof(HelpCommand) == name => JsonSerializer.Deserialize<HelpCommand>(serializedCommand),
                //var name when nameof(RepeatCommand) == name => JsonSerializer.Deserialize<RepeatCommand>(serializedCommand),
                var name when nameof(StartCommand) == name => JsonSerializer.Deserialize<StartCommand>(serializedCommand),
                var name when nameof(StudyNewCommand) == name => JsonSerializer.Deserialize<StudyNewCommand>(serializedCommand),
                _ => null
            };
        }

        public static IBotCommand ProduceNewForUpdate(Update update, IServiceProvider serviceProvider)
        {
            if (update.Type != UpdateType.Message)
            {
                return CreateHelpCommand(serviceProvider);
            }

            return update.Message.Text switch
            {
                var m when m.Contains(StartCommand.Name) => CreateStartCommand(serviceProvider),
                //var m when m.Contains(RepeatCommand.Name) => CreateRepeatCommand(serviceProvider),
                var m when m.Contains(StudyNewCommand.Name) => CreateStudyNewCommand(serviceProvider),
                _ => CreateHelpCommand(serviceProvider)
            };
        }

        private static StartCommand CreateStartCommand(IServiceProvider serviceProvider)
        {
            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
            return new StartCommand(serviceScopeFactory);
        }

        private static HelpCommand CreateHelpCommand(IServiceProvider serviceProvider)
        {
            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
            return new HelpCommand(serviceScopeFactory);
        }

        private static RepeatCommand CreateRepeatCommand(IServiceProvider serviceProvider)
        {
            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
            var logger = serviceProvider.GetService<ILogger<RepeatCommand>>()!;
            var repeatCommand = new RepeatCommand(serviceScopeFactory, logger);
            return repeatCommand;
        }

        private static StudyNewCommand CreateStudyNewCommand(IServiceProvider serviceProvider)
        {
            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
            var logger = serviceProvider.GetService<ILogger<StudyNewCommand>>()!;
            var studyNewCommand = new StudyNewCommand(serviceScopeFactory, logger);
            return studyNewCommand;
        }
    }
}