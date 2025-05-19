using DAL;

namespace Domain.Services.Size.DTO
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
