namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testtasks2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        BoardId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.BoardId);
            
            CreateTable(
                "dbo.Columns",
                c => new
                    {
                        ColumnId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Board_BoardId = c.Int(),
                    })
                .PrimaryKey(t => t.ColumnId)
                .ForeignKey("dbo.Boards", t => t.Board_BoardId)
                .Index(t => t.Board_BoardId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Deadline = c.DateTime(nullable: false),
                        Private = c.Boolean(nullable: false),
                        AssignedTo_UserId = c.Int(),
                        Column_ColumnId = c.Int(),
                        CreatedBy_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Users", t => t.AssignedTo_UserId)
                .ForeignKey("dbo.Columns", t => t.Column_ColumnId)
                .ForeignKey("dbo.Users", t => t.CreatedBy_UserId)
                .Index(t => t.AssignedTo_UserId)
                .Index(t => t.Column_ColumnId)
                .Index(t => t.CreatedBy_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "CreatedBy_UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "Column_ColumnId", "dbo.Columns");
            DropForeignKey("dbo.Tasks", "AssignedTo_UserId", "dbo.Users");
            DropForeignKey("dbo.Columns", "Board_BoardId", "dbo.Boards");
            DropIndex("dbo.Tasks", new[] { "CreatedBy_UserId" });
            DropIndex("dbo.Tasks", new[] { "Column_ColumnId" });
            DropIndex("dbo.Tasks", new[] { "AssignedTo_UserId" });
            DropIndex("dbo.Columns", new[] { "Board_BoardId" });
            DropTable("dbo.Tasks");
            DropTable("dbo.Columns");
            DropTable("dbo.Boards");
        }
    }
}
