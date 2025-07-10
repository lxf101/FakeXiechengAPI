namespace FakeXiechengAPI.Helper
{
    public class PaginationList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public PaginationList(int currentPage, int pageSize, List<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
        }
    }
}
