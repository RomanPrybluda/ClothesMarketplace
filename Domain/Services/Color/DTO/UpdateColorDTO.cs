using DAL;

namespace Domain
{
    public class UpdateColorDTO
    {
        public string Name { get; set; } = string.Empty;

        public void UpdateColor(Color color)
        {
            color.Name = Name;
        }
    }
}
