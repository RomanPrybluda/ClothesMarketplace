namespace DAL
{
    public class Image
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public bool IsMain { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
