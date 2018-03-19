namespace CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlobContentModificated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlobContents", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlobContents", "FileName");
        }
    }
}
