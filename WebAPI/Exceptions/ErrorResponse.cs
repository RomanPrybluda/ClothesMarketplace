using Newtonsoft.Json;

namespace WebAPI
{
    public class ErrorResponse
    {
        public string Type { get; set; }

        public string Description { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(string errorType, string description)
        {
            Type = errorType;
            Description = description;
        }

        public string Serialize() => JsonConvert.SerializeObject(this);

    }
}
