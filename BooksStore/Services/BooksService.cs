﻿using BooksStore.Interfaces;
using BooksStore.Models;
using BooksStore.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BooksStore.Services
{
    /// <summary>
    /// Сервис инкапсулирует бизнес-логику приложения (проверку пагинации, работу с изображениями) и вызовы репозитория <see cref="IBooksRepository"/>
    /// <para>В текущей реализации предполагается валидация модели на уровне слоя представления, иначе следует применить декоратор</para>
    /// <para>В распределенной системе для хранения файлов предполагается ипользование DFS, иначе необходимо использование поставщика файлов и локальное кэширование при запросах на чтение</para>
    /// </summary>
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _repository;
        private readonly IImagesService _imagesService;
        private readonly BooksServiceSettings _settings;
        private readonly ILogger<BooksService> _logger;

        public BooksService(IBooksRepository repository, IImagesService imagesService, BooksServiceSettings settings, ILogger<BooksService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _imagesService = imagesService;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> CreateBookAsync(Book book, Stream image = null, string imageContentType = null)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _logger.LogInformation($"Create book {book.Id} ({book.Title})");

            if (book.Id == default(Guid))
            {
                book.Id = Guid.NewGuid();
            }

            var containsImage = _imagesService.SaveImage(book, image, imageContentType);

            try
            {
                return await _repository.CreateBookAsync(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (containsImage)
                {
                    _imagesService.DeleteImage(book);
                }

                throw;
            }
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            if (bookId == default(Guid))
            {
                throw new ArgumentException("Empty guid", nameof(bookId));
            }

            _logger.LogInformation($"Delete book {bookId}");

            var book = await _repository.DeleteBookAsync(bookId);

            _imagesService.DeleteImage(book);
        }

        public async Task<Book> GetBookAsync(Guid bookId)
        {
            if (bookId == default(Guid))
            {
                throw new ArgumentException("Empty guid", nameof(bookId));
            }

            _logger.LogInformation($"Get book {bookId}");

            return await _repository.GetBookAsync(bookId);
        }

        public async Task<BooksPage> GetBooksAsync(Pagination pagination, SortOrder sortOrder = null)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            _logger.LogInformation($"Get books page {pagination.PageIndex} (size {pagination.PageSize}) order by {sortOrder?.Field} {sortOrder?.Sort}");

            if (pagination.PageSize == 0)
            {
                pagination.PageSize = _settings.DefaultPageSize;
            }

            if (pagination.PageSize > _settings.MaxPageSize)
            {
                throw new ArgumentException($"Page size is too large: {pagination.PageSize}");
            }

            return await _repository.GetBooksAsync(pagination, sortOrder);
        }

        public async Task UpdateBookAsync(Book book, Stream image = null, string imageContentType = null)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _logger.LogInformation($"Update book {book.Id}");

            var containsImage = _imagesService.SaveImage(book, image, imageContentType);

            try
            {
                await _repository.UpdateBookAsync(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (containsImage)
                {
                    _imagesService.DeleteImage(book);
                }

                throw;
            }
        }
    }
}
