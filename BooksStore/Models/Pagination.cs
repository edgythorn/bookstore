namespace BooksStore.Models
{
    public class Pagination
    {
        public Pagination()
        {

        }
        public Pagination(int index, int size)
        {
            PageIndex = index;
            PageSize = size;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
