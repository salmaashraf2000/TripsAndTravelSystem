namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentAndPostTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentDescription = c.String(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        applicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.applicationUser_Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.applicationUser_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripTitle = c.String(nullable: false, maxLength: 50),
                        TripDestenation = c.String(nullable: false, maxLength: 50),
                        TripDetails = c.String(nullable: false),
                        TripImage = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        TripDate = c.DateTime(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        RankLike = c.Int(nullable: false),
                        RankDislike = c.Int(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        applicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.applicationUser_Id)
                .Index(t => t.applicationUser_Id);
            
            AddColumn("dbo.AspNetUsers", "UserType", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "applicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "applicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "applicationUser_Id" });
            DropIndex("dbo.Comments", new[] { "applicationUser_Id" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropColumn("dbo.AspNetUsers", "Photo");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "UserType");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
