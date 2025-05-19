using DAL;

namespace Domain
{
    public class UpdateForWhomDTO
    {
        public string Name { get; set; } = string.Empty;

        public void UpdateForWhom(ForWhom forWhom)
        {
            forWhom.Name = Name;
        }
    }
}
