using BooksStore.Models;
using System;
using System.Threading.Tasks;

namespace BooksStore.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория книг
    /// <para>(предполагаются валидные данные на входе)</para>
    /// </summary>
    public interface IBooksRepository
    {
        Task<Guid> CreateBookAsync(Book book);

        Task<BooksPage> GetBooksAsync(Pagination pagination, SortOrder sortOrder = null);

        Task<Book> GetBookAsync(Guid bookId);

        Task UpdateBookAsync(Book book);

        Task<Book> DeleteBookAsync(Guid bookId);
    }
}