namespace RS.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Corev20 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SysAutoCode",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ScreenCode = c.String(nullable: false, maxLength: 5),
                        CodeFormat = c.String(nullable: false, maxLength: 20),
                        LastCodeNumber = c.Int(nullable: false),
                        CreateDT = c.DateTime(nullable: false),
                        UpdateDT = c.DateTime(),
                        CreateBy = c.Guid(nullable: false),
                        UpdateBy = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysAutoCodeLog",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CodeNumber = c.Int(nullable: false),
                        CodeGenerationDate = c.DateTime(nullable: false),
                        AutoCodeId = c.Guid(nullable: false),
                        GeneratedBy = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SysAutoCode", t => t.AutoCodeId)
                .ForeignKey("dbo.User", t => t.GeneratedBy)
                .Index(t => t.AutoCodeId)
                .Index(t => t.GeneratedBy);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        LastLoginDate = c.DateTime(),
                        IdentityUserId = c.Guid(nullable: false),
                        CreateDT = c.DateTime(nullable: false),
                        UpdateDT = c.DateTime(),
                        CreateBy = c.Guid(nullable: false),
                        UpdateBy = c.Guid(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysFile",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RefId = c.Guid(nullable: false),
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
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysAutoCodeLog", "GeneratedBy", "dbo.User");
            DropForeignKey("dbo.SysAutoCodeLog", "AutoCodeId", "dbo.SysAutoCode");
            DropIndex("dbo.SysAutoCodeLog", new[] { "GeneratedBy" });
            DropIndex("dbo.SysAutoCodeLog", new[] { "AutoCodeId" });
            DropTable("dbo.SysFile");
            DropTable("dbo.User");
            DropTable("dbo.SysAutoCodeLog");
            DropTable("dbo.SysAutoCode");
        }
    }
}
