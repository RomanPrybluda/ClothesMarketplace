namespace Domain
{
    public record RegistrationResponseDTO
    {
        public string Token { get; init; }

        public string RefreshToken { get; init; }

        public string Message { get; init; }
    }
}
