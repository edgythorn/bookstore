using BooksStore.Models;
using System;

namespace BooksStore.Tests.TestData
{
    static class InvalidModels
    {
        public static Book[] Get()
        {
            return new Book[]
            {
                new Book(),

                new TestBook("null title")
                {
                    Title = null,
                },
                new TestBook("empty title")
                {
                    Title = string.Empty,
                },
                new TestBook("over title")
                {
                    Title = new string('a', 91),
                },

                new TestBook("empty authors")
                {
                    Authors = new Author[0]
                },
                new TestBook("null authors")
                {
                    Authors = null
                },
                new TestBook("authors with empty givenname")
                {
                    Authors = new Author[]{ new TestAuthor { Givenname = string.Empty } }
                },
                new TestBook("authors with empty surname")
                {
                    Authors = new Author[]{ new TestAuthor { Surname = string.Empty } }
                },
                new TestBook("authors with null givenname")
                {
                    Authors = new Author[]{ new TestAuthor { Givenname = null } }
                },
                new TestBook("authors with null surname")
                {
                    Authors = new Author[]{ new TestAuthor { Surname = null } }
                },
                new TestBook("authors with over givenname")
                {
                    Authors = new Author[]{ new TestAuthor { Givenname = new string('a', 21) } }
                },
                new TestBook("authors with over surname")
                {
                    Authors = new Author[]{ new TestAuthor { Surname = new string('a', 21) } }
                },

                new TestBook("0 pages")
                {
                    PagesCount = 0
                },
                new TestBook("over pages")
                {
                    PagesCount = 10001
                },

                new TestBook("over publisher")
                {
                    Publisher = new string('a', 31),
                },

                new TestBook("less publish year")
                {
                    PublishYear = 1799
                },
                new TestBook("over publish year")
                {
                    PublishYear = DateTime.Now.Year + 1
                },

                new TestBook("null isbn")
                {
                    ISBN = null,
                },
                new TestBook("empty isbn")
                {
                    ISBN = string.Empty,
                },
                new TestBook("wrong isbn")
                {
                    ISBN = "5-93286-043-0"
                },
            };
        }
    }
}
