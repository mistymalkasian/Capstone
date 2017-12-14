namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newstuff : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "OrderContents");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderContents", c => c.String());
        }
    }
}
