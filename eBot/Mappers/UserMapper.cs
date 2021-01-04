using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using eBot.Commands;
using eBot.Data.Domain;
using eBot.Data.Persistent;
using eBot.DbContexts;
using eBot.Extensions;

namespace eBot.Mappers
{
    public static class UserMapper
    {
        public static UserDb Map(this User user)
        {
            return new UserDb
            {
                Id = user.Id,
                LastCommand = JsonSerializer.Serialize(user.LastCommand),
                LastCommandTypeName =  user.LastCommand?.GetType().Name,
                ElementsInProgress = user.ElementsInProgress.Map(RememberElementMapper.Map).ToList(),
                CompletelyRememberedElements = JsonSerializer.Serialize(user.CompletelyRememberedElements)
            };
        }
        
        public static User Map(this UserDb user, StudyContext studyContext)
        {
            var completelyRememberedElements = JsonSerializer.Deserialize<IList<long>>(user.CompletelyRememberedElements);
            var elementsInProgress = user.ElementsInProgress.Select(element =>
            {
                var vocabularyElement = studyContext.Vocabulary.Find(element.VocabularyElementId);
                var rememberElement = element.Map(vocabularyElement);
                return rememberElement;
            })
                .ToList();
            
            return new User(user.Id, elementsInProgress, completelyRememberedElements)
            {
                LastCommand = user.LastCommand.DeserializeCommandByName(user.LastCommandTypeName)!
            };
        }
    }
}