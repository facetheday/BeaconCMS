namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Group_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_ID)
                .Index(t => t.Group_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Group_ID", "dbo.Groups");
            DropIndex("dbo.Users", new[] { "Group_ID" });
            DropTable("dbo.Users");
        }
    }
}
