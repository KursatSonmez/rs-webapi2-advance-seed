namespace RS.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class B1RSC1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        CreateDT = c.DateTime(nullable: false),
                        UpdateDT = c.DateTime(),
                        CreateBy = c.Guid(nullable: false),
                        UpdateBy = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
