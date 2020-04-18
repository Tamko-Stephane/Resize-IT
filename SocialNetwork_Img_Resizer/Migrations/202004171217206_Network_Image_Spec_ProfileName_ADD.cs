namespace SocialNetwork_Img_Resizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Network_Image_Spec_ProfileName_ADD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Network_Image_Spec", "ProfileName", c => c.String());
            AddColumn("dbo.Network_Image_Spec", "Height", c => c.Int(nullable: false));
            AddColumn("dbo.Network_Image_Spec", "Width", c => c.Int(nullable: false));
            DropColumn("dbo.Network_Image_Spec", "MaxHeight");
            DropColumn("dbo.Network_Image_Spec", "MaxWidth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Network_Image_Spec", "MaxWidth", c => c.Int(nullable: false));
            AddColumn("dbo.Network_Image_Spec", "MaxHeight", c => c.Int(nullable: false));
            DropColumn("dbo.Network_Image_Spec", "Width");
            DropColumn("dbo.Network_Image_Spec", "Height");
            DropColumn("dbo.Network_Image_Spec", "ProfileName");
        }
    }
}
