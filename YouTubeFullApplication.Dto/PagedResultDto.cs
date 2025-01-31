namespace YouTubeFullApplication.Dto
{
    public interface IPagedResult
    {
        public int Page { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int TotalCount { get; }
    }

    public class PagedResultDto<T> : IPagedResult
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }
}
