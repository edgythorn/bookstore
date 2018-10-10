using BooksStore.Models;

namespace BooksStore.Tests.TestData
{
    class TestBook : Book
    {
        public string Description { get; set; }

        /// <summary>
        /// Создает экземпляр книги и заполняет обязательные поля минимально допустимыми значениями
        /// </summary>
        public TestBook(string description)
        {
            Description = description;

            Title = "a";
            PagesCount = 1;
            ISBN = "5-93286-045-6";
            Authors = new Author[]
            {
                new Author
                {
                    Givenname = "И",
                    Surname = "Ф"
                }
            };
        }
    }
}
