namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeaconLostMode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "BeaconLostMode", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notifications", "ProximityMode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "ProximityMode", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notifications", "BeaconLostMode");
        }
    }
}
