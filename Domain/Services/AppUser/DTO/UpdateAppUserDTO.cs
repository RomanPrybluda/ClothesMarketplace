namespace Domain
{
    public class UpdateAppUserDTO
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public void UpdateAppUser(DAL.AppUser user)
        {
            user.UserName = UserName ?? user.UserName;
            user.Email = Email ?? user.Email;
            user.FirstName = FirstName ?? user.FirstName;
            user.LastName = LastName ?? user.LastName;
        }
    }
}
