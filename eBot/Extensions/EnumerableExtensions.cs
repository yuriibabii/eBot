using System;
using System.Collections.Generic;
using System.Linq;

namespace eBot.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Map<TParameter, TResult>(
            this IEnumerable<TParameter> parameters,
            Func<TParameter, TResult> mapFunc)
        {
            return parameters.Select(mapFunc);
        }
    }
}