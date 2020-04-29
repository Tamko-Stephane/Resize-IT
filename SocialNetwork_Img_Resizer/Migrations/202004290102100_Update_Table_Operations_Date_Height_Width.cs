namespace SocialNetwork_Img_Resizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Table_Operations_Date_Height_Width : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operations", "Height", c => c.Single(nullable: false));
            AddColumn("dbo.Operations", "Width", c => c.Single(nullable: false));
            AddColumn("dbo.Operations", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operations", "Date");
            DropColumn("dbo.Operations", "Width");
            DropColumn("dbo.Operations", "Height");
        }
    }
}
