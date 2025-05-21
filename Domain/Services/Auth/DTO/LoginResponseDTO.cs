namespace Domain
{
    public record LoginResponseDTO
    {
        public bool Success { get; init; }

        public string? Token { get; init; }

        public string? RefreshToken { get; init; }

        public string? Message { get; init; }
    }
}
