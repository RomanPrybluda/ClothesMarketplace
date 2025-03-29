using Microsoft.AspNetCore.Identity;
using System;

namespace DAL
{
    public class UserRole : IdentityRole
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
