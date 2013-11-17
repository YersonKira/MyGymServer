namespace MyGym.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.APIObjects",
                c => new
                    {
                        ObjectId = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        JsonPost = c.String(),
                        JsonGet = c.String(),
                        Sample = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId);
            
            CreateTable(
                "dbo.APIParameters",
                c => new
                    {
                        ParamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Information = c.String(),
                        Definition = c.Int(nullable: false),
                        ObjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParamId)
                .ForeignKey("dbo.APIObjects", t => t.ObjectID, cascadeDelete: true)
                .Index(t => t.ObjectID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.APIParameters", new[] { "ObjectID" });
            DropForeignKey("dbo.APIParameters", "ObjectID", "dbo.APIObjects");
            DropTable("dbo.APIParameters");
            DropTable("dbo.APIObjects");
        }
    }
}
