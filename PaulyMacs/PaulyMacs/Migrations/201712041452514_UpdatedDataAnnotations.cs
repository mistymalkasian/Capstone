namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Balance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Balance", c => c.Int(nullable: false));
        }
    }
}
