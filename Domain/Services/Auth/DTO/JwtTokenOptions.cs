namespace Domain
{
    public record JwtTokenOptions
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string Key { get; init; }
    }
}
