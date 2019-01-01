using System.Linq;
using System;
using AutoSFaP.Models;

namespace AutoSFaP.Extensions
{
    public static class SortingExtension
    {
        public static IOrderedQueryable<T> Sort<T>(this SortField<T>[] sortFields, IQueryable<T> query) where T : class
        {
            if(sortFields == null)
            {
                throw new ArgumentNullException(nameof(sortFields));
            }
            if (sortFields.Length < 1)
            {
                throw new ArgumentException("sortFields can not be empty");
            }
            IOrderedQueryable<T> sortedData = null;
            foreach (var sortField in sortFields)
            {
                sortedData = sortedData == null ? sortField.SortBy(query) : sortField.SortThenBy(sortedData);
            }
            return sortedData;
        }

        public static IOrderedQueryable<T> SortBy<T>(this SortField<T> sortField, IQueryable<T> query) where T : class
        {
            return sortField.SortOrder == SortOrder.Ascending ? query.OrderBy(sortField.PropertyName.ToExpression<T>()) : query.OrderByDescending(sortField.PropertyName.ToExpression<T>());
        }

        private static IOrderedQueryable<T> SortThenBy<T>(this SortField<T> sortField, IOrderedQueryable<T> query) where T : class
        {
            return sortField.SortOrder == SortOrder.Ascending ? query.ThenBy(sortField.PropertyName.ToExpression<T>()) : query.ThenByDescending(sortField.PropertyName.ToExpression<T>());
        }
    }
}
