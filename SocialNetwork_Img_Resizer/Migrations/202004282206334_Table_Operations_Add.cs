namespace SocialNetwork_Img_Resizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_Operations_Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileUploaded = c.Binary(),
                        FileName = c.String(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Operations");
        }
    }
}
