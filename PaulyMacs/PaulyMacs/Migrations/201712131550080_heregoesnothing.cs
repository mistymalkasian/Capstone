namespace PaulyMacs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class heregoesnothing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Ratings", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Carts", new[] { "CustomerId" });
            DropIndex("dbo.Ratings", new[] { "CustomerId" });
            DropPrimaryKey("dbo.Customers");
            AddColumn("dbo.Carts", "Customer_UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Ratings", "Customer_UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Customers", "UserId");
            CreateIndex("dbo.Carts", "Customer_UserId");
            CreateIndex("dbo.Customers", "UserId");
            CreateIndex("dbo.Ratings", "Customer_UserId");
            AddForeignKey("dbo.Customers", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Carts", "Customer_UserId", "dbo.Customers", "UserId");
            AddForeignKey("dbo.Ratings", "Customer_UserId", "dbo.Customers", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Customer_UserId", "dbo.Customers");
            DropForeignKey("dbo.Carts", "Customer_UserId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "Customer_UserId" });
            DropIndex("dbo.Customers", new[] { "UserId" });
            DropIndex("dbo.Carts", new[] { "Customer_UserId" });
            DropPrimaryKey("dbo.Customers");
            AlterColumn("dbo.Customers", "UserId", c => c.String());
            AlterColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Ratings", "Customer_UserId");
            DropColumn("dbo.Carts", "Customer_UserId");
            AddPrimaryKey("dbo.Customers", "CustomerId");
            CreateIndex("dbo.Ratings", "CustomerId");
            CreateIndex("dbo.Carts", "CustomerId");
            AddForeignKey("dbo.Ratings", "CustomerId", "dbo.Customers", "CustomerId", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "CustomerId", "dbo.Customers", "CustomerId", cascadeDelete: true);
        }
    }
}
