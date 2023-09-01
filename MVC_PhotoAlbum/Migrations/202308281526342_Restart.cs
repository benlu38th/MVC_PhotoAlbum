namespace MVC_PhotoAlbum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Restart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhotoCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        PhotoUrl = c.String(nullable: false),
                        IsCover = c.Boolean(nullable: false),
                        InitDate = c.DateTime(),
                        PhotoCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhotoCategories", t => t.PhotoCategoryId, cascadeDelete: true)
                .Index(t => t.PhotoCategoryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 20),
                        ConfirmPassword = c.String(nullable: false, maxLength: 20),
                        Salt = c.String(nullable: false, maxLength: 20),
                        role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "PhotoCategoryId", "dbo.PhotoCategories");
            DropIndex("dbo.Photos", new[] { "PhotoCategoryId" });
            DropTable("dbo.Users");
            DropTable("dbo.Photos");
            DropTable("dbo.PhotoCategories");
        }
    }
}
