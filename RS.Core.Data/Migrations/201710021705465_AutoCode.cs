namespace RS.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutoCode",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ScreenCode = c.String(nullable: false, maxLength: 20),
                        CodeFormat = c.String(nullable: false, maxLength: 50),
                        LastCodeNumber = c.Int(nullable: false),
                        CreateDT = c.DateTime(nullable: false),
                        UpdateDT = c.DateTime(),
                        CreateBy = c.Guid(nullable: false),
                        UpdateBy = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AutoCodeLog",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CodeNumber = c.Int(nullable: false),
                        CodeGenerationDate = c.DateTime(nullable: false),
                        AutoCodeID = c.Guid(nullable: false),
                        GeneratedBy = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AutoCode", t => t.AutoCodeID)
                .ForeignKey("dbo.User", t => t.GeneratedBy)
                .Index(t => t.AutoCodeID)
                .Index(t => t.GeneratedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AutoCodeLog", "GeneratedBy", "dbo.User");
            DropForeignKey("dbo.AutoCodeLog", "AutoCodeID", "dbo.AutoCode");
            DropIndex("dbo.AutoCodeLog", new[] { "GeneratedBy" });
            DropIndex("dbo.AutoCodeLog", new[] { "AutoCodeID" });
            DropTable("dbo.AutoCodeLog");
            DropTable("dbo.AutoCode");
        }
    }
}
