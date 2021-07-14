namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllTabel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        like = c.Boolean(nullable: false),
                        Dislike = c.Boolean(nullable: false),
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
                "dbo.SavePosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaveDate = c.DateTime(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        applicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.applicationUser_Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.applicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SavePosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.SavePosts", "applicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Likes", "applicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SavePosts", new[] { "applicationUser_Id" });
            DropIndex("dbo.SavePosts", new[] { "PostId" });
            DropIndex("dbo.Likes", new[] { "applicationUser_Id" });
            DropIndex("dbo.Likes", new[] { "PostId" });
            DropTable("dbo.SavePosts");
            DropTable("dbo.Likes");
        }
    }
}
