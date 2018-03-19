namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeaconIDadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beacons", "BeaconID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beacons", "BeaconID");
        }
    }
}
