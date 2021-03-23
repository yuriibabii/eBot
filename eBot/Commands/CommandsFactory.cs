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
        }

        public static IBotCommand? DeserializeCommandByName(this string serializedCommand, string commandTypeName)
        {
            return commandTypeName switch
            {
                var name when nameof(HelpCommand) == name => JsonSerializer.Deserialize<HelpCommand>(serializedCommand),
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
                var m when m.Contains(StudyNewCommand.Name) || m.Contains($"???? {Strings.Commands.StudyName}") => CreateStudyNewCommand(serviceProvider),
                var m when m.Contains(HelpCommand.Name) || m.Contains($"???? {Strings.Commands.HelpName}") => CreateHelpCommand(serviceProvider),
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

        private static StudyNewCommand CreateStudyNewCommand(IServiceProvider serviceProvider)
        {
            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
            var logger = serviceProvider.GetService<ILogger<StudyNewCommand>>()!;
            var studyNewCommand = new StudyNewCommand(serviceScopeFactory, logger);
            return studyNewCommand;
        }
    }
}