namespace RS.Core.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RS.Core.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RS.Core.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RS.Core.Models.ApplicationDbContext context)
        {
            // Create ApplicationUser when not exist any user
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var PasswordHash = new PasswordHasher();
            if (!context.Users.Any())
            {
                var user = new ApplicationUser
                {
                    Id= "7cbf9971-7957-48dd-8198-3394a9bf0059",
                    UserName = "info@remmsoft.com",
                    Email = "info@remmsoft.com",
                    PasswordHash = PasswordHash.HashPassword("Test123*")
                };

                UserManager.Create(user);
            }
        }
    }
}
