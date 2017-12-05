namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemPicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "ItemPicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "ItemPicture");
        }
    }
}
