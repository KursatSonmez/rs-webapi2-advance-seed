namespace RS.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.File",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RefID = c.Guid(nullable: false),
                        ScreenCode = c.String(nullable: false, maxLength: 5),
                        OriginalName = c.String(nullable: false, maxLength: 30),
                        StorageName = c.String(nullable: false, maxLength: 50),
                        Extension = c.String(nullable: false, maxLength: 10),
                        Size = c.Long(),
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
            DropTable("dbo.File");
        }
    }
}
