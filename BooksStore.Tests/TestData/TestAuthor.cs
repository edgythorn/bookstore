using BooksStore.Models;

namespace BooksStore.Tests.TestData
{
    class TestAuthor : Author
    {
        /// <summary>
        /// Создает экземпляр автора и заполняет поля минимально допустимыми значениями
        /// </summary>
        public TestAuthor()
        {
            Givenname = "И";
            Surname = "Ф";
        }
    }
}
