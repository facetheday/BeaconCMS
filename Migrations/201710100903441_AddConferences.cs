namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConferences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conferences",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MajorID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Conferences");
        }
    }
}
