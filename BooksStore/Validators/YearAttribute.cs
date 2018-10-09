using System;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Validators
{
    internal class YearAttribute : ValidationAttribute
    {
        public YearAttribute(int min) : base()
        {
            MinValue = min;
        }

        public int MinValue { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return null;
            }

            if(!int.TryParse(value?.ToString(), out int intValue))
            {
                return new ValidationResult($"Недопустимое значение '{value ?? "null"}' для атрибута {nameof(YearAttribute)}");
            }

            if(intValue < MinValue || intValue > DateTime.Now.Year)
            {
                return new ValidationResult(ErrorMessage);
            }

            return null;
        }
    }
}