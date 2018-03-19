namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMajorMinorBeacon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beacons", "Major", c => c.Int(nullable: false));
            AddColumn("dbo.Beacons", "Minor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beacons", "Minor");
            DropColumn("dbo.Beacons", "Major");
        }
    }
}
