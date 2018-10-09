using BooksStore.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BooksStore.Interfaces
{
    /// <summary>
    /// Интерфейс службы хранения книг
    /// <para>(служба должна инкапсулировать работу с изображениями и прочую бизнес-логику)</para>
    /// </summary>
    public interface IBooksService
    {
        Task<Guid> CreateBookAsync(Book book, Stream image = null, string imageContentType = null);

        Task<BooksPage> GetBooksAsync(Pagination pagination, SortOrder sortOrder = null);

        Task<Book> GetBookAsync(Guid bookId);

        Task UpdateBookAsync(Book book, Stream image = null, string imageContentType = null);

        Task DeleteBookAsync(Guid bookId);
    }
}