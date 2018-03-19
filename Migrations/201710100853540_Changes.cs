namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Conferences");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Conferences",
                c => new
                    {
                        MajorID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.MajorID);
            
        }
    }
}
