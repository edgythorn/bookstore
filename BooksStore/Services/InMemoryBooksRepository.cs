using BooksStore.Interfaces;
using BooksStore.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BooksStore.Services
{
    public class InMemoryBooksRepository : IBooksRepository
    {
        private readonly ConcurrentDictionary<Guid, Book> _store = new ConcurrentDictionary<Guid, Book>();

        private readonly ILogger<InMemoryBooksRepository> _logger;

        private readonly Dictionary<string, Func<Book, object>> _sortOrderSelectors = new Dictionary<string, Func<Book, object>>(StringComparer.CurrentCultureIgnoreCase)
        {
            { nameof(Book.Title)      , b => b.Title },
            { nameof(Book.PublishYear), b => b.PublishYear },
        };

        public InMemoryBooksRepository(ILogger<InMemoryBooksRepository> logger)
        {
            _logger = logger;
        }

        public Task<Guid> CreateBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _logger.LogInformation($"Create book {book.Id} ({book.Title})");

            _store[book.Id] = book;
            return Task.FromResult(book.Id);
        }

        public Task<Book> DeleteBookAsync(Guid bookId)
        {
            if(bookId == default(Guid))
            {
                throw new ArgumentException("Empty guid", nameof(bookId));
            }

            _logger.LogInformation($"Delete book {bookId}");

            if (!_store.TryRemove(bookId, out Book book))
            {
                throw new KeyNotFoundException();
            }
            return Task.FromResult(book);
        }

        public Task<Book> GetBookAsync(Guid bookId)
        {
            if (bookId == default(Guid))
            {
                throw new ArgumentException("Empty guid", nameof(bookId));
            }

            _logger.LogInformation($"Get book {bookId}");

            if (!_store.TryGetValue(bookId, out Book book))
            {
                throw new KeyNotFoundException();
            }
            return Task.FromResult(book);
        }

        public Task<BooksPage> GetBooksAsync(Pagination pagination, SortOrder sortOrder = null)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            _logger.LogInformation($"Get books page {pagination.PageIndex} (size {pagination.PageSize}) order by {sortOrder?.Field} {sortOrder?.Sort}");

            IEnumerable<Book> query = _store.Values;

            if (sortOrder != null && sortOrder.Sort != SortType.Unsorted)
            {
                if (!_sortOrderSelectors.TryGetValue(sortOrder.Field, out Func<Book, object> selector))
                {
                    throw new NotSupportedException($"Сортировка по полю '{sortOrder.Field}' не поддерживается");
                }

                query = sortOrder.Sort == SortType.Ascending ? query.OrderBy(selector) : query.OrderByDescending(selector);
            }

            var books = query
                .Skip(pagination.PageSize * pagination.PageIndex)
                .Take(pagination.PageSize)
                .ToArray();

            var result = new BooksPage
            {
                Books = books,
                Total = _store.Count
            };

            return Task.FromResult(result);
        }

        public Task UpdateBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _logger.LogInformation($"Update book {book.Id}");

            if (!_store.TryGetValue(book.Id, out Book existing))
            {
                throw new KeyNotFoundException();
            }

            if (!_store.TryUpdate(book.Id, book, existing))
            {
                throw new Exception("Не удалось обновить книгу"); //TODO custom exception
            }

            return Task.CompletedTask;
        }
    }
}
