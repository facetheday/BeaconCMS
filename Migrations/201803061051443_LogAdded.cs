namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Beacon_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                        Content_Id = c.Guid(nullable: false),
                        Group_Id = c.Int(nullable: false),
                        Url_Id = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
