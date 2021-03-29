using System.Collections.Generic;
using System.Linq;
using eBot.Data.Domain;

namespace eBot.Extensions
{
    public static class RememberElementsExtensions
    {
        public static VocabStudyElement? GetBestToRepeatElement(this IEnumerable<VocabStudyElement> rememberElements)
        {
            var bestToLearnElement = rememberElements.OrderBy(element => element.Progress)
                .ThenBy(element => element.LastTimeRepeated)
                .FirstOrDefault();

            return bestToLearnElement;
        }
    }
}