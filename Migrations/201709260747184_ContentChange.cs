namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TextContent = c.String(),
                        ImageContent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Beacons", "Content_ID", c => c.Guid());
            CreateIndex("dbo.Beacons", "Content_ID");
            AddForeignKey("dbo.Beacons", "Content_ID", "dbo.Contents", "ID");
            DropColumn("dbo.Beacons", "Content_TextContent");
            DropColumn("dbo.Beacons", "Content_ImageContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beacons", "Content_ImageContent", c => c.Int(nullable: false));
            AddColumn("dbo.Beacons", "Content_TextContent", c => c.String());
            DropForeignKey("dbo.Beacons", "Content_ID", "dbo.Contents");
            DropIndex("dbo.Beacons", new[] { "Content_ID" });
            DropColumn("dbo.Beacons", "Content_ID");
            DropTable("dbo.Contents");
        }
    }
}
