namespace YouTubeFullApplication.Dto
{
    public abstract class RequestBaseDto
    {
        private int pageSize = 30;
        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;
                if (pageSize < 10) pageSize = 10;
            }
        }

        private int page = 1;
        public int Page
        {
            get => page;
            set
            {
                page = value;
                if (page < 1) page = 1;
            }
        }

        private readonly string orderDefault;

        private string order = string.Empty;
        public string Order
        {
            get
            {
                return string.IsNullOrWhiteSpace(order) ? orderDefault : order;
            }
            set => order = value.ToLower().Trim();
        }

        public bool RetriveDeleted { get; set; }

        public RequestBaseDto(string orderDefault)
        {
            this.orderDefault = orderDefault.ToLower();
        }
    }
}
