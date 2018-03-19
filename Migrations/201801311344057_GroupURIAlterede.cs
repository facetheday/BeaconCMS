namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupURIAlterede : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupURIs", "URL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupURIs", "URL");
        }
    }
}
