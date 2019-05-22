namespace JTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'54ec3f30-3f90-4fcd-9f29-f4ef4c45d8d3', N'CanManageResolvedTickets')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'14d86451-f8e7-46e5-ab59-aa296fcc5f85', N'admin@jticket.com', 0, N'AOQHuYjH+lVahuTva/NyYzObJi7HrI72EOormQEPlRKrYeb1h8Ro6+uKd6A717pxqA==', N'56b6e424-cd86-4b0c-ac27-a13c233edaca', NULL, 0, 0, NULL, 1, 0, N'admin@jticket.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'c1f8574a-a09b-4374-a4a4-5d8f62ac7a42', N'guest@jticket.com', 0, N'AKV63yWCZxvKnpF8DeeGlRp3dmcTsQ5CiBxzRBbJdawVAIAHMI4aJaUCMZcB6lvH1Q==', N'bc723a20-edaf-4641-bf40-8a3a2845ede2', NULL, 0, 0, NULL, 1, 0, N'guest@jticket.com')
                ");
        }
        
        public override void Down()
        {
        }
    }
}
