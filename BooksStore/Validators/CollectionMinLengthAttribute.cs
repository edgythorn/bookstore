using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Validators
{
    internal class CollectionMinLengthAttribute : ValidationAttribute
    {
        public CollectionMinLengthAttribute(int min)
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

            var collection = value as ICollection;
            if(collection == null)
            {
                return new ValidationResult($"Недопустимый тип данных '{value.GetType().FullName}' для атрибута {nameof(CollectionMinLengthAttribute)}");
            }

            if (collection.Count < MinValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return null;
        }
    }
}