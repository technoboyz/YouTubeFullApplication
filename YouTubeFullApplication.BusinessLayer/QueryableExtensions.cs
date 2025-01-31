using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace YouTubeFullApplication.BusinessLayer
{
    internal static class QueryableExtensions
    {
        public static IQueryable<T> AddFilter<T>(this IQueryable<T> queryable, string? value, string property, bool contains = true)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string _value = value.Replace("\"", "").ToLower();
                if (contains)
                {
                    queryable = queryable.Where($"{property}.ToLower().Contains(@0)", new object[] { _value });
                }
                else
                {
                    queryable = queryable.Where($"{property}.ToLower() = @0", new object[] { _value });
                }
            }
            return queryable;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, bool condiction)
        {
            return condiction ? queryable.Where(predicate) : queryable;
        }
    }
}
