using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Auth.DTO
{
    public record RegistrationResponseDTO
    {
        public string Token { get; init; }

        public string RefreshToken { get; init; }

        public string Message { get; init; }
    }
}
