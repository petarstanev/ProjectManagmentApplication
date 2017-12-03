namespace ProjectManagmentApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_User_Model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Users", "PasswordHash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PasswordHash", c => c.String());
            AlterColumn("dbo.Users", "Name", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            DropColumn("dbo.Users", "Password");
        }
    }
}
