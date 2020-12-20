using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eBot.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<TSource?> FirstOrNullAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default)
        where TSource : class
            => await source
                .FirstOrDefaultAsync(predicate, cancellationToken);
    }
}