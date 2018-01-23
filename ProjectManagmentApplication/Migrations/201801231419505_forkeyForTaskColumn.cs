namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forkeyForTaskColumn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "Column_ColumnId", "dbo.Columns");
            DropIndex("dbo.Tasks", new[] { "Column_ColumnId" });
            RenameColumn(table: "dbo.Tasks", name: "Column_ColumnId", newName: "ColumnId");
            AlterColumn("dbo.Tasks", "ColumnId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "ColumnId");
            AddForeignKey("dbo.Tasks", "ColumnId", "dbo.Columns", "ColumnId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ColumnId", "dbo.Columns");
            DropIndex("dbo.Tasks", new[] { "ColumnId" });
            AlterColumn("dbo.Tasks", "ColumnId", c => c.Int());
            RenameColumn(table: "dbo.Tasks", name: "ColumnId", newName: "Column_ColumnId");
            CreateIndex("dbo.Tasks", "Column_ColumnId");
            AddForeignKey("dbo.Tasks", "Column_ColumnId", "dbo.Columns", "ColumnId");
        }
    }
}
