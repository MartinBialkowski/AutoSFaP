using AutoSFaP.Extensions;
using AutoSFaP.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSFaP
{
    public class DataLimiter<T> : IDataLimiter<T> where T : class
    {
        public IQueryable<T> LimitData(IQueryable<T> query, SortField<T>[] sortFields, FilterField<T>[] filterFields)
        {
            if (filterFields != null)
            {
                query = filterFields.Filter(query);
            }

            if (sortFields != null)
            {
                query = sortFields.Sort(query);
            }
            return query;
        }

        public async Task<PagedResult<T>> LimitDataAsync(IQueryable<T> query, SortField<T>[] sortFields, FilterField<T>[] filterFields, Paging paging)
        {
            var limitedQuery = LimitData(query, sortFields, filterFields);
            return new PagedResult<T>()
            {
                Results = await paging.Page(limitedQuery).ToList(),
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfRecords = query.Count(),
                TotalNumberOfPages = (int)Math.Ceiling(query.Count() / (double)paging.PageLimit)
            };
        }
    }
}
