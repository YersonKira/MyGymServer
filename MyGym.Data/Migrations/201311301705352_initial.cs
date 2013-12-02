namespace MyGym.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imagens",
                c => new
                    {
                        ImagenID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Image = c.Binary(storeType: "image"),
                        Ejercicio_EjercicioID = c.Int(),
                    })
                .PrimaryKey(t => t.ImagenID)
                .ForeignKey("dbo.EJERCICIO", t => t.Ejercicio_EjercicioID)
                .Index(t => t.Ejercicio_EjercicioID);
            
            AddColumn("dbo.SE_CONFORMA", "Descipcion", c => c.String());
            AlterColumn("dbo.EJERCICIO", "Distancia", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Imagens", new[] { "Ejercicio_EjercicioID" });
            DropForeignKey("dbo.Imagens", "Ejercicio_EjercicioID", "dbo.EJERCICIO");
            AlterColumn("dbo.EJERCICIO", "Distancia", c => c.Int(nullable: false));
            DropColumn("dbo.SE_CONFORMA", "Descipcion");
            DropTable("dbo.Imagens");
        }
    }
}
