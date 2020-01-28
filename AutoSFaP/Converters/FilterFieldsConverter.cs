using AutoSFaP.Models;
using System.Linq;

namespace AutoSFaP.Converters
{
    public static class FilterFieldsConverter<TSource, TResult> where TResult : class
    {
        public static FilterField<TResult>[] Convert(TSource source)
        {
            return (from property in typeof(TSource).GetProperties()
                    let propertyValue = property.GetValue(source)
                    where propertyValue != null
                    select new FilterField<TResult>
                    {
                        PropertyName = property.Name,
                        FilterValue = propertyValue
                    }).ToArray();
        }
    }
}
