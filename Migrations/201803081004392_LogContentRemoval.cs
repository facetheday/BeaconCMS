namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogContentRemoval : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Logs", "Content_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "Content_Id", c => c.Guid(nullable: false));
        }
    }
}
