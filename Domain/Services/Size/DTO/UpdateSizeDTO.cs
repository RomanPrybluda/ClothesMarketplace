using DAL;

namespace Domain
{
    public class UpdateSizeDTO
    {
        public string Name { get; set; } = string.Empty;

        public void UpdateSize(ProductSize size)
        {
            size.Name = Name;
        }
    }
}
