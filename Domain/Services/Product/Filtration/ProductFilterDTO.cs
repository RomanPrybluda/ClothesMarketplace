namespace Domain
{
    public class ProductFilterDTO
    {
        public Guid? BrandId { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? ColorId { get; set; }

        public Guid? ProductSizeId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? SearchQuery { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public ProductSortBy? SortBy { get; set; }

        public SortDirection? SortDirection { get; set; }
    }


}
