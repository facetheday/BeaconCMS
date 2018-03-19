namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupURItoURL : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupURIs", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.GroupURIs", "Content_ID", "dbo.Contents");
            DropIndex("dbo.GroupURIs", new[] { "Group_ID" });
            DropIndex("dbo.GroupURIs", new[] { "Content_ID" });
            CreateTable(
                "dbo.URLs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UrlValue = c.String(),
                        Group_ID = c.Int(),
                        Content_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_ID)
                .ForeignKey("dbo.Contents", t => t.Content_ID)
                .Index(t => t.Group_ID)
                .Index(t => t.Content_ID);
            
            DropColumn("dbo.Contents", "URL");
            DropTable("dbo.GroupURIs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GroupURIs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                        Group_ID = c.Int(),
                        Content_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Contents", "URL", c => c.String());
            DropForeignKey("dbo.URLs", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.URLs", "Group_ID", "dbo.Groups");
            DropIndex("dbo.URLs", new[] { "Content_ID" });
            DropIndex("dbo.URLs", new[] { "Group_ID" });
            DropTable("dbo.URLs");
            CreateIndex("dbo.GroupURIs", "Content_ID");
            CreateIndex("dbo.GroupURIs", "Group_ID");
            AddForeignKey("dbo.GroupURIs", "Content_ID", "dbo.Contents", "ID");
            AddForeignKey("dbo.GroupURIs", "Group_ID", "dbo.Groups", "ID");
        }
    }
}
