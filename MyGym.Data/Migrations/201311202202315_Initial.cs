namespace MyGym.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.USUARIO",
                c => new
                    {
                        UsuarioID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Nick = c.String(),
                        Paterno = c.String(),
                        Materno = c.String(),
                        Nombre = c.String(),
                        Sexo = c.Int(nullable: false),
                        Peso = c.Double(nullable: false),
                        Estatura = c.Double(nullable: false),
                        ComplexionFisica = c.Int(nullable: false),
                        FechaNacimiento = c.DateTime(nullable: false, storeType: "datetime2"),
                        Nivel = c.Int(nullable: false),
                        FactorActividad = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.UsuarioID);
            
            CreateTable(
                "dbo.DIETA",
                c => new
                    {
                        DietaID = c.Int(nullable: false, identity: true),
                        Dia = c.Int(nullable: false),
                        Calorias = c.Double(nullable: false),
                        Proteinas = c.Double(nullable: false),
                        Grasas = c.Double(nullable: false),
                        HidratosCarbono = c.Double(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DietaID)
                .ForeignKey("dbo.USUARIO", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.TIENE",
                c => new
                    {
                        DietaID = c.Int(nullable: false),
                        RecomendacionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DietaID, t.RecomendacionID })
                .ForeignKey("dbo.DIETA", t => t.DietaID, cascadeDelete: true)
                .ForeignKey("dbo.RECOMENDACION", t => t.RecomendacionID, cascadeDelete: true)
                .Index(t => t.DietaID)
                .Index(t => t.RecomendacionID);
            
            CreateTable(
                "dbo.RECOMENDACION",
                c => new
                    {
                        RecomendacionID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Preparacion = c.String(),
                        Calorias = c.Double(nullable: false),
                        Proteinas = c.Double(nullable: false),
                        Grasas = c.Double(nullable: false),
                        HidratosDeCarbono = c.Double(nullable: false),
                        Image = c.Binary(storeType: "image"),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.RecomendacionID);
            
            CreateTable(
                "dbo.SE_CONFORMA",
                c => new
                    {
                        RecomendacionID = c.Int(nullable: false),
                        AlimentoID = c.Int(nullable: false),
                        Cantidad = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecomendacionID, t.AlimentoID })
                .ForeignKey("dbo.ALIMENTO", t => t.AlimentoID, cascadeDelete: true)
                .ForeignKey("dbo.RECOMENDACION", t => t.RecomendacionID, cascadeDelete: true)
                .Index(t => t.AlimentoID)
                .Index(t => t.RecomendacionID);
            
            CreateTable(
                "dbo.ALIMENTO",
                c => new
                    {
                        AlimentoID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Calorias = c.Double(nullable: false),
                        Proteinas = c.Double(nullable: false),
                        Grasas = c.Double(nullable: false),
                        HidratosDeCarbono = c.Double(nullable: false),
                        GrupoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AlimentoID)
                .ForeignKey("dbo.GRUPO", t => t.GrupoID, cascadeDelete: true)
                .Index(t => t.GrupoID);
            
            CreateTable(
                "dbo.GRUPO",
                c => new
                    {
                        GrupoID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        RecomendacionMinima = c.Int(nullable: false),
                        RecomendacionMaxima = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GrupoID);
            
            CreateTable(
                "dbo.SE_RECOMIENDA_EN",
                c => new
                    {
                        TiempoDeComidaID = c.Int(nullable: false),
                        RecomendacionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TiempoDeComidaID, t.RecomendacionID })
                .ForeignKey("dbo.TIEMPO_DE_COMIDA", t => t.TiempoDeComidaID, cascadeDelete: true)
                .ForeignKey("dbo.RECOMENDACION", t => t.RecomendacionID, cascadeDelete: true)
                .Index(t => t.TiempoDeComidaID)
                .Index(t => t.RecomendacionID);
            
            CreateTable(
                "dbo.TIEMPO_DE_COMIDA",
                c => new
                    {
                        TiempoDeComidaID = c.Int(nullable: false, identity: true),
                        Nombre = c.Int(nullable: false),
                        HoraInicio = c.Time(nullable: false),
                        HoraFinal = c.Time(nullable: false),
                    })
                .PrimaryKey(t => t.TiempoDeComidaID);
            
            CreateTable(
                "dbo.PREFERENCIA_TIEMPO_COMIDA",
                c => new
                    {
                        TiempoDeComidaID = c.Int(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TiempoDeComidaID, t.UsuarioID })
                .ForeignKey("dbo.TIEMPO_DE_COMIDA", t => t.TiempoDeComidaID, cascadeDelete: true)
                .ForeignKey("dbo.USUARIO", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.TiempoDeComidaID)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.HISTORIAL",
                c => new
                    {
                        HistorialID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Peso = c.Double(nullable: false),
                        Estatura = c.Double(nullable: false),
                        EstadoNutricional = c.Int(nullable: false),
                        IMC = c.Double(nullable: false),
                        Calorias = c.Double(nullable: false),
                        Proteinas = c.Double(nullable: false),
                        Grasas = c.Double(nullable: false),
                        HidratosCarbono = c.Double(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HistorialID)
                .ForeignKey("dbo.USUARIO", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.RUTINA",
                c => new
                    {
                        RutinaID = c.Int(nullable: false, identity: true),
                        Nivel = c.Int(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RutinaID)
                .ForeignKey("dbo.USUARIO", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.EJERCICIO",
                c => new
                    {
                        EjercicioID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Repeticiones = c.Int(nullable: false),
                        Sets = c.Int(nullable: false),
                        Duracion = c.Int(nullable: false),
                        Distancia = c.Int(nullable: false),
                        Peso = c.Int(nullable: false),
                        Calorias = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EjercicioID);
            
            CreateTable(
                "dbo.Instruccions",
                c => new
                    {
                        InstruccionID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Step = c.Int(nullable: false),
                        Ejercicio_EjercicioID = c.Int(),
                    })
                .PrimaryKey(t => t.InstruccionID)
                .ForeignKey("dbo.EJERCICIO", t => t.Ejercicio_EjercicioID)
                .Index(t => t.Ejercicio_EjercicioID);
            
            CreateTable(
                "dbo.ACTIVIDAD",
                c => new
                    {
                        ActividadID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        RutinaID = c.Int(nullable: false),
                        EjercicioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActividadID)
                .ForeignKey("dbo.RUTINA", t => t.RutinaID, cascadeDelete: true)
                .ForeignKey("dbo.EJERCICIO", t => t.EjercicioID, cascadeDelete: true)
                .Index(t => t.RutinaID)
                .Index(t => t.EjercicioID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ACTIVIDAD", new[] { "EjercicioID" });
            DropIndex("dbo.ACTIVIDAD", new[] { "RutinaID" });
            DropIndex("dbo.Instruccions", new[] { "Ejercicio_EjercicioID" });
            DropIndex("dbo.RUTINA", new[] { "UsuarioID" });
            DropIndex("dbo.HISTORIAL", new[] { "UsuarioID" });
            DropIndex("dbo.PREFERENCIA_TIEMPO_COMIDA", new[] { "UsuarioID" });
            DropIndex("dbo.PREFERENCIA_TIEMPO_COMIDA", new[] { "TiempoDeComidaID" });
            DropIndex("dbo.SE_RECOMIENDA_EN", new[] { "RecomendacionID" });
            DropIndex("dbo.SE_RECOMIENDA_EN", new[] { "TiempoDeComidaID" });
            DropIndex("dbo.ALIMENTO", new[] { "GrupoID" });
            DropIndex("dbo.SE_CONFORMA", new[] { "RecomendacionID" });
            DropIndex("dbo.SE_CONFORMA", new[] { "AlimentoID" });
            DropIndex("dbo.TIENE", new[] { "RecomendacionID" });
            DropIndex("dbo.TIENE", new[] { "DietaID" });
            DropIndex("dbo.DIETA", new[] { "UsuarioID" });
            DropForeignKey("dbo.ACTIVIDAD", "EjercicioID", "dbo.EJERCICIO");
            DropForeignKey("dbo.ACTIVIDAD", "RutinaID", "dbo.RUTINA");
            DropForeignKey("dbo.Instruccions", "Ejercicio_EjercicioID", "dbo.EJERCICIO");
            DropForeignKey("dbo.RUTINA", "UsuarioID", "dbo.USUARIO");
            DropForeignKey("dbo.HISTORIAL", "UsuarioID", "dbo.USUARIO");
            DropForeignKey("dbo.PREFERENCIA_TIEMPO_COMIDA", "UsuarioID", "dbo.USUARIO");
            DropForeignKey("dbo.PREFERENCIA_TIEMPO_COMIDA", "TiempoDeComidaID", "dbo.TIEMPO_DE_COMIDA");
            DropForeignKey("dbo.SE_RECOMIENDA_EN", "RecomendacionID", "dbo.RECOMENDACION");
            DropForeignKey("dbo.SE_RECOMIENDA_EN", "TiempoDeComidaID", "dbo.TIEMPO_DE_COMIDA");
            DropForeignKey("dbo.ALIMENTO", "GrupoID", "dbo.GRUPO");
            DropForeignKey("dbo.SE_CONFORMA", "RecomendacionID", "dbo.RECOMENDACION");
            DropForeignKey("dbo.SE_CONFORMA", "AlimentoID", "dbo.ALIMENTO");
            DropForeignKey("dbo.TIENE", "RecomendacionID", "dbo.RECOMENDACION");
            DropForeignKey("dbo.TIENE", "DietaID", "dbo.DIETA");
            DropForeignKey("dbo.DIETA", "UsuarioID", "dbo.USUARIO");
            DropTable("dbo.ACTIVIDAD");
            DropTable("dbo.Instruccions");
            DropTable("dbo.EJERCICIO");
            DropTable("dbo.RUTINA");
            DropTable("dbo.HISTORIAL");
            DropTable("dbo.PREFERENCIA_TIEMPO_COMIDA");
            DropTable("dbo.TIEMPO_DE_COMIDA");
            DropTable("dbo.SE_RECOMIENDA_EN");
            DropTable("dbo.GRUPO");
            DropTable("dbo.ALIMENTO");
            DropTable("dbo.SE_CONFORMA");
            DropTable("dbo.RECOMENDACION");
            DropTable("dbo.TIENE");
            DropTable("dbo.DIETA");
            DropTable("dbo.USUARIO");
        }
    }
}
