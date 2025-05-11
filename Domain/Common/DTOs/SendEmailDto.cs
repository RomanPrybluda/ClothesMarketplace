using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DTOs
{
    public record SendEmailDto
    {
        public string Email { get; init; }
        public string Subject { get; init; }
        public string Body { get; init; }
        public string Name {  get; init; }
    }
}
