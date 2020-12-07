using System.Collections.Generic;
using System.Linq;
using eBot.Models;

namespace eBot.Extensions
{
    public static class RememberElementsExtensions
    {
        public static RememberElement? GetBestToRepeatElement(this IEnumerable<RememberElement> rememberElements)
        {
            var bestToLearnElement = rememberElements.OrderBy(element => element.Progress)
                .ThenBy(element => element.LastTimeRepeated)
                .FirstOrDefault();

            return bestToLearnElement;
        }
    }
}