namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DeviceId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DeviceId");
        }
    }
}
