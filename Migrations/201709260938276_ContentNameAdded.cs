namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentNameAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "Name", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "Name");
        }
    }
}
