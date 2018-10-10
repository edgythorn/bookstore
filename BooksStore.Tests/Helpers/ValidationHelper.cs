using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Tests
{
    static class ValidationHelper
    {
        public static List<ValidationResult> Validate(this Book model)
        {
            var results = Validate((object)model);

            if (model.Authors == null)
            {
                return results;
            }

            foreach (var item in model.Authors)
            {
                var res = item.Validate();
                if (res != null)
                {
                    results = results ?? new List<ValidationResult>();
                    results.AddRange(res);
                }
            }

            return results;
        }

        public static List<ValidationResult> Validate(this object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, context, results, true))
            {
                return results;
            }

            return null;
        }
    }
}
