using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public static class RoleRegistry
    {
        public static readonly IdentityRole Admin = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        public static readonly IdentityRole User = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "User",
            NormalizedName = "USER"
        };

        private static readonly IReadOnlyList<IdentityRole> SupportedRoles;

        static RoleRegistry()
        {
            SupportedRoles = ParseFields();
        }

        public static IReadOnlyList<IdentityRole> GetRoles() => SupportedRoles;

        private static IReadOnlyList<IdentityRole> ParseFields()
        {
            Type type = typeof(RoleRegistry);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            var roles = new List<IdentityRole>();
            foreach (FieldInfo field in fields)
            {
                var role = (IdentityRole)field.GetValue(null);
                roles.Add(role);
            }
            return roles;
        }
    }
}
