using System;

namespace AutoSFaP.Models
{
    public class FilterField<T> where T : class
    {
        public string PropertyName
        {
            get => propertyName;
	        set
            {
                if (value != propertyName)
                {
                    ValidatePropertyName(value);
                    propertyName = value;
                    PropertyType = ModelType.GetProperty(propertyName).PropertyType;
                }
            }
        }
        public object FilterValue
        {
            get => filterValue;
	        set
            {
                if (value != filterValue)
                {
                    ValidateFilterValue(value);
                    filterValue = value;
                }
            }
        }
        public Type PropertyType { get; set; }
        protected Type ModelType { get; }
        private string propertyName;
        private object filterValue;

        public FilterField()
        {
            ModelType = typeof(T);
        }

        public FilterField(string propertyName, object filterValue)
        {
            ModelType = typeof(T);
            PropertyName = propertyName;
            FilterValue = filterValue;
        }

        protected void ValidatePropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (ModelType.GetProperty(propertyName) == null)
            {
                throw new ArgumentException($"{propertyName} is not a public property of {ModelType.FullName}");
            }
        }

        protected void ValidateFilterValue(object filterValue)
        {
            if (filterValue == null)
            {
                throw new ArgumentNullException(nameof(filterValue));
            }
            if (filterValue.GetType() != PropertyType)
            {
                throw new ArgumentException($"Value of {propertyName} is of type {filterValue.GetType().FullName}, expected type {PropertyType.FullName}");
            }
        }
    }
}
