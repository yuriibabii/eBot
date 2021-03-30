using System.Collections.Generic;
using eBot.Commands;

namespace eBot.Data.Domain
{
    public class User
    {
        public User(
            long id,
            IList<IStudyElement>? elementsInProgress = default,
            IList<long>? completelyRememberedElements = default)
        {
            Id = id;
            ElementsInProgress = elementsInProgress ?? new List<IStudyElement>();
            CompletelyRememberedElements = completelyRememberedElements ?? new List<long>();
        }

        public long Id { get; }

        public long NextElementToStudyId => ElementsInProgress.Count + CompletelyRememberedElements.Count + 1;

        public IList<IStudyElement> ElementsInProgress { get; }

        public IList<long> CompletelyRememberedElements { get; }

        //TODO: Doesn't work now
        public IBotCommand? LastCommand { get; set; }
    }
}