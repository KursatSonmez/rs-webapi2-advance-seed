namespace RS.Core.Data.Migrations
{
    using RS.Core.Domain;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RS.Core.Data.RSCoreDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RS.Core.Data.RSCoreDBContext context)
        {
            // Create User when not exist any user
            if (!context.User.Any())
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "info@remmsoft.com",
                    Phone = "+90 224 372 9143",
                    Email = "info@remmsoft.com",
                    IdentityUserId = Guid.Parse("7cbf9971-7957-48dd-8198-3394a9bf0059"),
                    CreateDT = DateTime.Now
                };
                user.CreateBy = user.Id;

                context.User.Add(user);
                context.SaveChanges();
            }
        }
    }
}
