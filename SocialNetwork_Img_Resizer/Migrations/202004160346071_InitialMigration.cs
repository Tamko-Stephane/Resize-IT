namespace SocialNetwork_Img_Resizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Network_Image_Spec",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaxHeight = c.Int(nullable: false),
                        MaxWidth = c.Int(nullable: false),
                        Social_Network_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Social_Network", t => t.Social_Network_Id)
                .Index(t => t.Social_Network_Id);
            
            CreateTable(
                "dbo.Social_Network",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Icon = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Network_Image_Spec", "Social_Network_Id", "dbo.Social_Network");
            DropIndex("dbo.Network_Image_Spec", new[] { "Social_Network_Id" });
            DropTable("dbo.Social_Network");
            DropTable("dbo.Network_Image_Spec");
        }
    }
}
