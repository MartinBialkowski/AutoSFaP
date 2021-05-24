using AutoSFaP.Models;
using System;
using System.Reflection;

namespace AutoSFaP.Converters
{
    public static class SortFieldsConverter<T> where T : class
    {
        public static SortField<T>[] Convert(string source)
        {
            var sortData = source.Split(',');
            var result = new SortField<T>[sortData.Length];
            for (var i = 0; i < sortData.Length; i++)
            {
                result[i] = ConvertToSortField(sortData[i]);
            }

            return result;
        }

        private static SortField<T> ConvertToSortField(string sortData)
        {
            var propertyInfo = FindPropertyCaseInsensitive(sortData);

            var sortField = new SortField<T>
            {
                SortOrder = sortData.EndsWith("-") ? SortOrder.Descending : SortOrder.Ascending,
                PropertyName = propertyInfo.Name
            };

            return sortField;
        }

        private static PropertyInfo FindPropertyCaseInsensitive(string sortData)
        {
            var propertyName = sortData.Trim('-', '+');
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Could not find property with name {propertyName}");
            }

            return propertyInfo;
        }
    }
}
