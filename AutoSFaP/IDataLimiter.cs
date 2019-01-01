using AutoSFaP.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSFaP
{
    public interface IDataLimiter<T> where T : class
    {
        IQueryable<T> LimitData(IQueryable<T> query, SortField<T>[] sortFields, FilterField<T>[] filterFields);
        Task<PagedResult<T>> LimitDataAsync(IQueryable<T> query, SortField<T>[] sortFields, FilterField<T>[] filterFields, Paging paging);

    }
}
