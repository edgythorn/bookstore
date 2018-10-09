using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BooksStore.Validators
{
    public static class ISBNValidator
    {
        public static bool IsValid(string isbn)
        {
            if (!Regex.IsMatch(isbn, @"(^978-(\d+-){3}\d$)|(^(?!978-)(\d+-){3}[\dx]$)", RegexOptions.IgnoreCase))
            {
                return false;
            }

            isbn = Regex.Replace(isbn, @"[^\dX]", string.Empty, RegexOptions.IgnoreCase);

            if (isbn.Length != 10 && isbn.Length != 13)
            {
                return false;
            }

            var digits = isbn.Select(ch => ch.ToString()).ToArray();

            if (isbn.Length == 10)
            {
                if (digits[9].Equals("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    digits[9] = "10";
                }
                var sum = CalculateChecksum(digits, (digit, index) => (10 - index) * digit);
                return (sum.IsValid && sum.Value % 11 == 0);
            }
            else // EAN-13
            {
                var sum = CalculateChecksum(digits, (digit, index) => index % 2 == 0 ? digit : digit * 3);
                return (sum.IsValid && sum.Value % 10 == 0);
            }
        }

        private static (bool IsValid, int Value) CalculateChecksum(string[] digits, Func<byte, int, int> iteration)
        {
            int sum = 0;
            for (var i = 0; i < digits.Length; i++)
            {
                if (!byte.TryParse(digits[i], out byte digit))
                {
                    return (false, 0);
                }

                sum += iteration(digit, i);
            }
            return (true, sum);
        }
    }
}

