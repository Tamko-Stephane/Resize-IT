namespace SocialNetwork_Img_Resizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Network_Image_Spec_ImageCDNSrc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Network_Image_Spec", "ImageCDNSrc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Network_Image_Spec", "ImageCDNSrc");
        }
    }
}
