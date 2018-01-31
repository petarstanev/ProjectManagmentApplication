namespace ProjectManagementApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImagesFK : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "TaskId", "dbo.Tasks");
            DropIndex("dbo.Images", new[] { "TaskId" });
            DropTable("dbo.Images");
        }
    }
}
