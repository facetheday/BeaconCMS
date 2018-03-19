namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupURI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupURIs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group_ID = c.Int(),
                        Content_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_ID)
                .ForeignKey("dbo.Contents", t => t.Content_ID)
                .Index(t => t.Group_ID)
                .Index(t => t.Content_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupURIs", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.GroupURIs", "Group_ID", "dbo.Groups");
            DropIndex("dbo.GroupURIs", new[] { "Content_ID" });
            DropIndex("dbo.GroupURIs", new[] { "Group_ID" });
            DropTable("dbo.GroupURIs");
        }
    }
}
