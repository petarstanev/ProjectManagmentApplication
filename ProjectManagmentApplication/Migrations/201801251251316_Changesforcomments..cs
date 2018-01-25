namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changesforcomments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        TaskId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropTable("dbo.Comments");
        }
    }
}
