namespace SocialNetwork_Img_Resizer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SEED_Network_Image_Spec : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT [dbo].[Network_Image_Spec] ON
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (1, 1, N'Profile Picture', 180, 180)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (2, 1, N'Cover Photo', 315, 851)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (3, 1, N'Linked Image', 246, 470)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (4, 1, N'In-Stream Square', 470, 470)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (5, 2, N'Profile Photo', 400, 400)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (6, 2, N'Header Photo', 500, 1500)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (7, 2, N'In-Stream Tall', 506, 506)
INSERT INTO [dbo].[Network_Image_Spec] ([Id], [Social_Network_Id], [ProfileName], [Height], [Width]) VALUES (8, 2, N'In-Stream Wide', 253, 506)
SET IDENTITY_INSERT [dbo].[Network_Image_Spec] OFF
");
        }

        public override void Down()
        {
            Sql("DELETE FROM [dbo].[Network_Image_Spec]");
        }
    }
}
