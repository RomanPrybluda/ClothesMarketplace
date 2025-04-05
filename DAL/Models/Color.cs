namespace DAL
{
    public class Color
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Product>? Products { get; set; }
    }
}
