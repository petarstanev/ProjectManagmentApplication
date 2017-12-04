namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testaasdasd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
