namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Jops", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Jops", new[] { "CategoryId" });
            RenameColumn(table: "dbo.Comments", name: "applicationUser_Id", newName: "UserId");
            RenameColumn(table: "dbo.Posts", name: "applicationUser_Id", newName: "UserId");
            RenameColumn(table: "dbo.Likes", name: "applicationUser_Id", newName: "UserId");
            RenameColumn(table: "dbo.SavePosts", name: "applicationUser_Id", newName: "UserId");
            RenameIndex(table: "dbo.Comments", name: "IX_applicationUser_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Posts", name: "IX_applicationUser_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Likes", name: "IX_applicationUser_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.SavePosts", name: "IX_applicationUser_Id", newName: "IX_UserId");
            DropColumn("dbo.Comments", "ApplicationUserId");
            DropColumn("dbo.Posts", "ApplicationUserId");
            DropColumn("dbo.Likes", "ApplicationUserId");
            DropColumn("dbo.SavePosts", "ApplicationUserId");
            DropTable("dbo.Categories");
            DropTable("dbo.Jops");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Jops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JopTitle = c.String(),
                        JopContent = c.String(),
                        JopImage = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SavePosts", "ApplicationUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Likes", "ApplicationUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "ApplicationUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "ApplicationUserId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.SavePosts", name: "IX_UserId", newName: "IX_applicationUser_Id");
            RenameIndex(table: "dbo.Likes", name: "IX_UserId", newName: "IX_applicationUser_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_applicationUser_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_UserId", newName: "IX_applicationUser_Id");
            RenameColumn(table: "dbo.SavePosts", name: "UserId", newName: "applicationUser_Id");
            RenameColumn(table: "dbo.Likes", name: "UserId", newName: "applicationUser_Id");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "applicationUser_Id");
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "applicationUser_Id");
            CreateIndex("dbo.Jops", "CategoryId");
            AddForeignKey("dbo.Jops", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
