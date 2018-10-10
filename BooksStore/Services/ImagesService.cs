using BooksStore.Interfaces;
using BooksStore.Models;
using BooksStore.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;

namespace BooksStore.Services
{
    public class ImagesService : IImagesService
    {
        private readonly BooksServiceSettings _settings;
        private readonly ILogger<ImagesService> _logger;

        public ImagesService(BooksServiceSettings settings, ILogger<ImagesService> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void DeleteImage(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _logger.LogInformation($"DeleteImage book id '{book.Id}'");

            DeleteFile(book.Image);
            DeleteFile(book.ImagePreview);
        }

        public bool SaveImage(Book book, Stream stream, string imageContentType)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (stream == null)
            {
                return false;
            }

            _logger.LogInformation($"SaveImage book id '{book.Id}'");

            ValidateImage(stream, imageContentType, out string extension);

            //TODO При очень большом количестве файлов необходима структуризация каталога
            var directory = Path.Combine(_settings.ImagesRootPath, _settings.ImagesRelativePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileName = $"{book.Id}.{extension}";
            var previewFileName = $"{book.Id}-preview.{extension}";
            var imagePath = Path.Combine(directory, fileName);
            var previewPath = Path.Combine(directory, previewFileName);

            //TODO Оптимизация и превью, только если размер превышает заданный порог
            //TODO При высоких нагрузках возможны проблемы с памятью - использовать SemaphoreSlim при работе с изображениями
            using (var original = Image.FromStream(stream))
            using (var optimized = original.Resize(_settings.ImageOptimizedHeight))
            using (var preview = original.Resize(_settings.ImagePreviewHeigt))
            {
                optimized.Save(imagePath);
                _logger.LogInformation($"Saved: {imagePath}");

                preview.Save(previewPath);
                _logger.LogInformation($"Saved: {imagePath}");
            }

            book.Image = $"{_settings.ImagesRelativePath}/{fileName}";
            book.ImagePreview = $"{_settings.ImagesRelativePath}/{previewFileName}";

            _logger.LogInformation($"Set {nameof(Book.Image)} property for book id '{book.Id}': {book.Image}");
            _logger.LogInformation($"Set {nameof(Book.ImagePreview)} property for book id '{book.Id}': {book.ImagePreview}");


            return true;

        }

        private void ValidateImage(Stream stream, string imageContentType, out string extension)
        {
            if (!_settings.AllowedContentTypes.TryGetValue(imageContentType, out extension))
            {
                throw new NotSupportedException($"Content type {imageContentType} is not supported");
            }

            if (stream.Length == 0)
            {
                throw new ArgumentException("Empty file");
            }

            if (stream.Length > _settings.MaxImageSize)
            {
                throw new ArgumentException("File is too large");
            }
        }

        private void DeleteFile(string imageRelativePath)
        {
            if (string.IsNullOrEmpty(imageRelativePath))
            {
                return;
            }

            var path = Path.Combine(_settings.ImagesRootPath, imageRelativePath);
            if (File.Exists(path))
            {
                File.Delete(path);
                _logger.LogInformation($"Deleted: {path}");
            }
        }
    }
}
