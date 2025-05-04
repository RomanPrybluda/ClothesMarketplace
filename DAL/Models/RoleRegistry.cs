using Microsoft.AspNetCore.Identity;
using System.Reflection;

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

        public static bool IsValidRole(string roleName)
        {
            return SupportedRoles.Any(role => role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
