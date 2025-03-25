namespace DAL
{
    public class ProductSize
    {
        public Guid Id { get; set; }

        public string Value { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = new();
    }
}
