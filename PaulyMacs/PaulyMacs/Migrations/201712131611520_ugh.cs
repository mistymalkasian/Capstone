namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ugh : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "CustomerId");
            DropColumn("dbo.Orders", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false));
        }
    }
}
