using AutoSFaP.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoSFaP.Extensions
{
    public static class PagingExtension
    {
        public static IAsyncEnumerable<T> Page<T>(this Paging paging, IQueryable<T> query) where T : class
        {
            return query.Skip(paging.Offset).Take(paging.PageLimit).ToAsyncEnumerable();
        }
    }
}
