using DAL;

namespace Domain
{
    public class ForWhomDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;

        public static ForWhomDTO FromForWhom(ForWhom forWhom)
        {
            return new ForWhomDTO
            {
                Id = forWhom.Id,
                Name = forWhom.Name
            };
        }
    }
}