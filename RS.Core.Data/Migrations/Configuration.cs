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
            //Veritaban�nda admin kullan�c�s�na ait bir kay�t yoksa, ilgili kayd�n atamas�n� yapmaktad�r.
            if (!context.User.Any(x => x.Email == "info@remmsoft.com"))
            {
                var user = new User
                {
                    ID = Guid.NewGuid(),
                    Name = "info@remmsoft.com",
                    Phone = "+90 224 372 9143",
                    Email = "info@remmsoft.com",
                    IdentityUserID = Guid.Parse("7cbf9971-7957-48dd-8198-3394a9bf0059"),
                    CreateDT = DateTime.Now
                };
                user.CreateBy = user.ID;

                context.User.Add(user);
                context.SaveChanges();
            }
        }
    }
}
