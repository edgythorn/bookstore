using BooksStore.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Book
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(90, ErrorMessage = "Название книги должно быть не более 90 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Список авторов обязателен")]
        [CollectionMinLength(1, ErrorMessage = "Список авторов не должен быть пустым")]
        public Author[] Authors { get; set; }

        [Required(ErrorMessage = "Число страниц обязательно")]
        [Range(1,10000, ErrorMessage = "Число страниц должно быть от 1 до 10000")]
        public int PagesCount { get; set; }

        [StringLength(30, ErrorMessage = "Название издательства должно быть не более 30 символов")]
        public string Publisher { get; set; }

        [Year(1800, ErrorMessage = "Год публикации должен быть не раньше 1800 и не больше текущего года")]
        public int? PublishYear { get; set; }

        [Required(ErrorMessage = "Номер ISBN обязателен")]
        [ISBN(ErrorMessage = "Номер ISBN должен соответствовать установленному формату")]
        public string ISBN { get; set; }

        /// <summary>
        /// Путь к изображению для детализированного просмотра
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Путь к изображению для просмотра в таблице
        /// </summary>
        public string ImagePreview { get; set; }
    }
}
