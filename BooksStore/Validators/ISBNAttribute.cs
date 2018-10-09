using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BooksStore.Validators
{
    internal class ISBNAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            return ISBNValidator.IsValid(value.ToString()) ? null : new ValidationResult(ErrorMessage);
        }
    }
}