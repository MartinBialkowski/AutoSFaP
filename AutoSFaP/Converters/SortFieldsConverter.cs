using AutoMapper;
using AutoSFaP.Models;

namespace AutoSFaP.Converters
{
    public class SortFieldsConverter<T> : ITypeConverter<string, SortField<T>[]> where T : class
    {
        public SortField<T>[] Convert(string source, SortField<T>[] destination, ResolutionContext context)
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
            var sortField = new SortField<T>
            {
                SortOrder = sortData.EndsWith("-") ? SortOrder.Descending : SortOrder.Ascending,
                PropertyName = sortData.Trim('-', '+')
            };

            return sortField;
        }
    }
}
