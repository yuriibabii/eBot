using System;
using System.Threading.Tasks;

namespace eBot.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TResult?> MapAsync<TParameter, TResult>(this Task<TParameter?> task, Func<TParameter, TResult> mapFunc)
            where TParameter : class
            where TResult : class
        {
            var result = await task;
            if (result == null)
            {
                return null;
            }
            
            var mappedResult = mapFunc(result);
            return mappedResult;
        }
    }
}