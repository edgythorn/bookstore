using BooksStore.Models;
using System;
using System.Linq;

namespace BooksStore.Tests.TestData
{
    static class ValidModels
    {
        public static Book[] Get()
        {
            return new Book[]
            {
                new TestBook("required min"),
                new TestBook("required max")
                {
                    Title = new string('a', 90),
                    PagesCount = 10000,
                    Authors = new Author[] {
                        new Author
                        {
                            Givenname = new string('a', 20),
                            Surname = new string('a', 20)
                        }
                    }
                },
                new TestBook("optionals min")
                {
                    Publisher = string.Empty,
                    PublishYear = 1800
                },
                new TestBook("optionals max")
                {
                    Publisher = new string('a', 30),
                    PublishYear = DateTime.Now.Year
                },
                new TestBook("many authors")
                {
                    Authors = Enumerable.Repeat(new TestAuthor(), 100).ToArray()
                },
            };
        }
    }
}
