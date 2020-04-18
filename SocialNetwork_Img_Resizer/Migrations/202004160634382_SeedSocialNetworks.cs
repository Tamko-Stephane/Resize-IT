namespace SocialNetwork_Img_Resizer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SeedSocialNetworks : DbMigration
    {
        public override void Up()
        {
            Sql(@"
SET IDENTITY_INSERT [dbo].[Social_Network] ON
INSERT INTO [dbo].[Social_Network] ([Id], [Name], [Icon]) VALUES (1, N'facebook', N'fa fa-facebook-square')
INSERT INTO [dbo].[Social_Network] ([Id], [Name], [Icon]) VALUES (2, N'twitter', N'fa fa-twitter-square')
SET IDENTITY_INSERT [dbo].[Social_Network] OFF
");
        }

        public override void Down()
        {
            Sql(@"
    DELETE FROM [dbo].[Social_Network]
");
        }
    }
}
