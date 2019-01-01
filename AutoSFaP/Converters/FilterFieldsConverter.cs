using AutoMapper;
using AutoSFaP.Models;
using System.Linq;

namespace AutoSFaP.Converters
{
    public class FilterFieldsConverter<TSource, TResult> : ITypeConverter<TSource, FilterField<TResult>[]> where TResult : class
    {
        public FilterField<TResult>[] Convert(TSource source, FilterField<TResult>[] destination, ResolutionContext context)
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
