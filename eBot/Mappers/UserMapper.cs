using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using eBot.Data.Domain;
using eBot.Data.Persistent;
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
                LastCommand = user.LastCommand,
                ElementsInProgress = user.ElementsInProgress.Map(RememberElementMapper.Map).ToList(),
                CompletelyRememberedElements = JsonSerializer.Serialize(user.CompletelyRememberedElements)
            };
        }
        
        public static User Map(this UserDb user)
        {
            var completelyRememberedElements = JsonSerializer.Deserialize<IList<int>>(user.CompletelyRememberedElements);
            var elementsInProgress = user.ElementsInProgress.Select(element =>
            {
                var vocabularyElement = element. 
            });
            
            
            return new User(user.Id, .Map(RememberElementMapper.Map()), completelyRememberedElements)
            {
                LastCommand = user.LastCommand
            };
        }
    }
}