namespace MVC_PhotoAlbum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSaltMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Salt", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Salt", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
