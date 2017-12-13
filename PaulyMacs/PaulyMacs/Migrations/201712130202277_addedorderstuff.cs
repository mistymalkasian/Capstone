namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedorderstuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "MenuItemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "MenuItemId");
        }
    }
}
