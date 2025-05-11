using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DTOs
{
    public record BrevoApiSettings
    {
        public string ApiKey { get; init; }
        public string SenderEmail {  get; init; }
        public string SenderName { get; init; }
    }
}
