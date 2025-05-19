using DAL;

namespace Domain
{
    public class CreateForWhomDTO
    {
        public string Name { get; set; } = string.Empty;

        public static ForWhom ToForWhom(CreateForWhomDTO request)
        {
            return new ForWhom
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };
        }
    }
}