using System.Collections.Generic;

namespace BooksStore.Settings
{
    /// <summary>
    /// Настройки сервиса
    /// </summary>
    public class BooksServiceSettings
    {
        /// <summary>
        /// Размер страницы данных по умолчанию
        /// </summary>
        public int DefaultPageSize { get; set; }

        /// <summary>
        /// Максимальный размер страницы данных
        /// </summary>
        public int MaxPageSize { get; set; }

        /// <summary>
        /// Корневой путь каталога изображений
        /// </summary>
        public string ImagesRootPath { get; set; }

        /// <summary>
        /// Относительный путь каталога изображений (включается в путь файла изображения модели)
        /// </summary>
        public string ImagesRelativePath { get; set; }

        /// <summary>
        /// Допустимые типы файлов. Ключ - content-type, значение - расширение файла
        /// </summary>
        public Dictionary<string, string> AllowedContentTypes { get; set; }

        /// <summary>
        /// Максимальный размер файла изображения
        /// </summary>
        public long MaxImageSize { get; set; }

        /// <summary>
        /// Высота изображения для оптимизации размера файла в пикселах
        /// </summary>
        public float ImageOptimizedHeight { get; set; }

        /// <summary>
        /// Высота изображения для просмотра в таблице в пикселах
        /// </summary>
        public float ImagePreviewHeigt { get; set; }
    }
}
