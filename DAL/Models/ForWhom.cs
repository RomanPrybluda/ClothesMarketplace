namespace DAL
{
    public class ForWhom
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Product>? Products { get; set; }
    }
}
