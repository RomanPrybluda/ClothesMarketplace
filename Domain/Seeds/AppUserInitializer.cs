using Bogus;
using DAL;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUserInitializer
    {
        private readonly UserManager<AppUser> _userManager;
        private const int NeedsUsersQuantity = 50;

        public AppUserInitializer(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public void InitializeAppUsers()
        {
            if (_userManager.Users.Count() >= NeedsUsersQuantity)
                return;

            var faker = new Faker("en");

            for (int i = 0; i < NeedsUsersQuantity; i++)
            {
                var email = faker.Internet.Email();
                var userName = faker.Internet.UserName();

                var user = _userManager.FindByEmailAsync(email).Result;

                if (user == null)
                {
                    var newUser = new AppUser
                    {
                        UserName = userName,
                        Email = email,
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName(),
                        Age = faker.Random.Int(18, 60),
                        EmailConfirmed = true
                    };

                    var createResult = _userManager.CreateAsync(newUser, "User123!").Result;

                    if (createResult.Succeeded)
                    {
                        _userManager.AddToRoleAsync(newUser, "User").Wait();
                    }
                }
            }
        }
    }
}
