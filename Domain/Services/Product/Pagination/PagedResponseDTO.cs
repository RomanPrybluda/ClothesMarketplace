namespace Domain
{
    public class PagedResponseDTO<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public PagedResponseDTO(IEnumerable<T> items, int totalItems, int skip, int take)
        {
            Items = items;
            TotalItems = totalItems;
            Skip = skip;
            Take = take;
        }
    }

}
