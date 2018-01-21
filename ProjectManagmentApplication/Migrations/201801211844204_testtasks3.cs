namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testtasks3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boards", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Boards", "BoardViewModel_BoardId", c => c.Int());
            AddColumn("dbo.Boards", "SelectedBoard_BoardId", c => c.Int());
            CreateIndex("dbo.Boards", "BoardViewModel_BoardId");
            CreateIndex("dbo.Boards", "SelectedBoard_BoardId");
            AddForeignKey("dbo.Boards", "BoardViewModel_BoardId", "dbo.Boards", "BoardId");
            AddForeignKey("dbo.Boards", "SelectedBoard_BoardId", "dbo.Boards", "BoardId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Boards", "SelectedBoard_BoardId", "dbo.Boards");
            DropForeignKey("dbo.Boards", "BoardViewModel_BoardId", "dbo.Boards");
            DropIndex("dbo.Boards", new[] { "SelectedBoard_BoardId" });
            DropIndex("dbo.Boards", new[] { "BoardViewModel_BoardId" });
            DropColumn("dbo.Boards", "SelectedBoard_BoardId");
            DropColumn("dbo.Boards", "BoardViewModel_BoardId");
            DropColumn("dbo.Boards", "Discriminator");
        }
    }
}
