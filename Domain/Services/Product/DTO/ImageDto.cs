namespace Domain.Services.Product.DTO
{
    public record ImageDto
    {
        public string Url { get; init; }
        public bool IsMain { get; init; }
    }
}