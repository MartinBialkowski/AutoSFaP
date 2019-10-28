using AutoSFaP.Extensions;
using AutoSFaP.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSFaP
{
    public class DataLimiter<T> : IDataLimiter<T> where T : class
    {
        public IQueryable<T> LimitData(IQueryable<T> baseQuery, SortField<T>[] sortFields, FilterField<T>[] filterFields)
        {
            if (filterFields != null)
            {
                baseQuery = filterFields.Filter(baseQuery);
            }

            if (sortFields != null)
            {
                baseQuery = sortFields.Sort(baseQuery);
            }
            return baseQuery;
        }

        public async Task<PagedResult<T>> LimitDataAsync(IQueryable<T> baseQuery, SortField<T>[] sortFields, FilterField<T>[] filterFields, Paging paging)
        {
            var limitedQuery = LimitData(baseQuery, sortFields, filterFields);
            return await CreatePagedResultAsync(limitedQuery, paging);
        }

        public async Task<PagedResult<T>> LimitDistinctDataAsync(IQueryable<T> baseQuery, SortField<T>[] sortFields, FilterField<T>[] filterFields, Paging paging)
        {
            var limitedQuery = LimitData(baseQuery, sortFields, filterFields).Distinct();
            return await CreatePagedResultAsync(limitedQuery, paging);
        }

        private async Task<PagedResult<T>> CreatePagedResultAsync(IQueryable<T> limitedQuery, Paging paging)
        {
            return new PagedResult<T>()
            {
                Results = await paging.Page(limitedQuery).ToList(),
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfRecords = limitedQuery.Count(),
                TotalNumberOfPages = (int)Math.Ceiling(limitedQuery.Count() / (double)paging.PageLimit)
            };
        }
    }
}
