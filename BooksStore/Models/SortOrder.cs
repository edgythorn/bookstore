namespace BooksStore.Models
{
    public class SortOrder
    {
        public SortOrder()
        {

        }
        public SortOrder(string field, SortType sort)
        {
            Field = field;
            Sort = sort;
        }

        public string Field { get; set; }

        public SortType Sort { get; set; }
    }
}
