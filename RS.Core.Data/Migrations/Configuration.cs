namespace RS.Core.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<RS.Core.Data.RSCoreDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RS.Core.Data.RSCoreDBContext context)
        {
        }
    }
}
