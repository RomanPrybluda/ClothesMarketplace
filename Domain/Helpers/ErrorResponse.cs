namespace Domain.Helpers
{
    public record ErrorResponse
    {
        public List<string> Errors { get; init; }
    }
}