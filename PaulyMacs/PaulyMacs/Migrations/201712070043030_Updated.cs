namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        CartItemId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CartId = c.Int(nullable: false),
                        MenuItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemId)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemId, cascadeDelete: true)
                .Index(t => t.CartId)
                .Index(t => t.MenuItemId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        isCheckedOut = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        PagesId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Slug = c.String(),
                        Body = c.String(),
                        Sorting = c.Int(nullable: false),
                        HasSidebar = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PagesId);
            
            CreateTable(
                "dbo.Sidebars",
                c => new
                    {
                        SidebarId = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.SidebarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "MenuItemId", "dbo.MenuItems");
            DropForeignKey("dbo.CartItems", "CartId", "dbo.Carts");
            DropForeignKey("dbo.Carts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Carts", new[] { "CustomerId" });
            DropIndex("dbo.CartItems", new[] { "MenuItemId" });
            DropIndex("dbo.CartItems", new[] { "CartId" });
            DropTable("dbo.Sidebars");
            DropTable("dbo.Pages");
            DropTable("dbo.Carts");
            DropTable("dbo.CartItems");
        }
    }
}
