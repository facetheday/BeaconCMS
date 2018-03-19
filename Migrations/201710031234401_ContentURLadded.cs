namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentURLadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "URL", c => c.String());
            DropColumn("dbo.Contents", "TextContent");
            DropColumn("dbo.Contents", "ImageContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contents", "ImageContent", c => c.Int(nullable: false));
            AddColumn("dbo.Contents", "TextContent", c => c.String());
            DropColumn("dbo.Contents", "URL");
        }
    }
}
