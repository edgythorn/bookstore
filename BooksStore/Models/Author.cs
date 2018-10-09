using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Author
    {
        [Required(ErrorMessage = "Имя автора обязательно")]
        [StringLength(20, ErrorMessage = "Имя автора должно быть не более 20 символов")]
        public string Givenname { get; set; }

        [Required(ErrorMessage = "Фамилия автора обязательна")]
        [StringLength(20, ErrorMessage = "Фамилия автора должна быть не более 20 символов")]
        public string Surname { get; set; }
    }
}
