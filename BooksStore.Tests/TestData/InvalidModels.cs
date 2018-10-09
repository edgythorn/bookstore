using BooksStore.Models;

namespace BooksStore.Tests.TestData
{
    static class InvalidModels
    {
        public static Book[] Get()
        {
            return new Book[]
            {
                new Book(),
                new Book
                {

                },
                //TODO InvalidModels
            };
        }
    }
}
