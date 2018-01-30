namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        BoardId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        BoardViewModel_BoardId = c.Int(),
                        SelectedBoard_BoardId = c.Int(),
                    })
                .PrimaryKey(t => t.BoardId)
                .ForeignKey("dbo.Boards", t => t.BoardViewModel_BoardId)
                .ForeignKey("dbo.Boards", t => t.SelectedBoard_BoardId)
                .Index(t => t.BoardViewModel_BoardId)
                .Index(t => t.SelectedBoard_BoardId);
            
            CreateTable(
                "dbo.Columns",
                c => new
                    {
                        ColumnId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        BoardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ColumnId)
                .ForeignKey("dbo.Boards", t => t.BoardId, cascadeDelete: true)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Deadline = c.DateTime(nullable: false),
                        Private = c.Boolean(nullable: false),
                        ColumnId = c.Int(nullable: false),
                        AssignedTo_UserId = c.Int(),
                        CreatedBy_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Users", t => t.AssignedTo_UserId)
                .ForeignKey("dbo.Columns", t => t.ColumnId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy_UserId)
                .Index(t => t.ColumnId)
                .Index(t => t.AssignedTo_UserId)
                .Index(t => t.CreatedBy_UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
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
            DropForeignKey("dbo.Boards", "SelectedBoard_BoardId", "dbo.Boards");
            DropForeignKey("dbo.Boards", "BoardViewModel_BoardId", "dbo.Boards");
            DropForeignKey("dbo.Tasks", "CreatedBy_UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "ColumnId", "dbo.Columns");
            DropForeignKey("dbo.Tasks", "AssignedTo_UserId", "dbo.Users");
            DropForeignKey("dbo.Columns", "BoardId", "dbo.Boards");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropIndex("dbo.Tasks", new[] { "CreatedBy_UserId" });
            DropIndex("dbo.Tasks", new[] { "AssignedTo_UserId" });
            DropIndex("dbo.Tasks", new[] { "ColumnId" });
            DropIndex("dbo.Columns", new[] { "BoardId" });
            DropIndex("dbo.Boards", new[] { "SelectedBoard_BoardId" });
            DropIndex("dbo.Boards", new[] { "BoardViewModel_BoardId" });
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
            DropTable("dbo.Columns");
            DropTable("dbo.Boards");
        }
    }
}
