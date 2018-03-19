namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                        DisplayFrequency = c.Int(nullable: false),
                        ProximityMode = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Beacons", "Notification_ID", c => c.Int());
            CreateIndex("dbo.Beacons", "Notification_ID");
            AddForeignKey("dbo.Beacons", "Notification_ID", "dbo.Notifications", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Beacons", "Notification_ID", "dbo.Notifications");
            DropIndex("dbo.Beacons", new[] { "Notification_ID" });
            DropColumn("dbo.Beacons", "Notification_ID");
            DropTable("dbo.Notifications");
        }
    }
}
