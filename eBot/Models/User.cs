using System.Collections.Generic;
using eBot.Commands;

namespace eBot.Models
{
    public class User
    {
        public User(
            long id,
            IList<RememberElement>? elementsInProgress = default,
            IList<int>? completelyRememberedElements = default)
        {
            Id = id;
            ElementsInProgress = elementsInProgress ?? new List<RememberElement>();
            CompletelyRememberedElements = completelyRememberedElements ?? new List<int>();
        }
        
        public long Id { get; }

        public int NextElementToStudyId => ElementsInProgress.Count + CompletelyRememberedElements.Count + 1;
        
        public IList<RememberElement> ElementsInProgress { get; }
        
        public IList<int> CompletelyRememberedElements { get; }
        
        public ICommand LastCommand { get; set; } 
    }
}