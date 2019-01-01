using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoSFaP.Extensions
{
    public static class StringExtension
    {
        public static Expression<Func<T, object>> ToExpression<T>(this string propertyName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, propertyName);
            Expression conversion = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }

        public static Expression<Func<T, bool>> ToConstraintExpression<T>(this string propertyName, object filterValue)
        {
            MethodInfo containsMethod;
            Expression argument;
            if (filterValue is string)
            {
                // After Net Core 2.1 release possible bug
                // containsMethod = typeof(string).GetMethod("Contains");
                containsMethod = typeof(string).GetMethods().First(x => x.Name == "Contains");
                argument = Expression.Constant(filterValue);
            }
            else
            {
                containsMethod = typeof(object).GetMethods().First(x => x.Name == "Equals" && x.GetParameters().Length == 1);
                argument = Expression.Constant(filterValue, typeof(object));
            }
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var expression = Expression.Call(property, containsMethod, argument);
            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
    }
}
