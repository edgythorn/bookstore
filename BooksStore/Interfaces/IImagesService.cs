using BooksStore.Models;
using System.IO;

namespace BooksStore.Interfaces
{
    public interface IImagesService
    {
        bool SaveImage(Book book, Stream image, string imageContentType);

        void DeleteImage(Book book);
    }
}
