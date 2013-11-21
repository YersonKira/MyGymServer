using MyGym.Common.Enum;
using MyGym.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGym.Data
{
    public class MyGymContext : DbContext
    {
        private static MyGymContext db = new MyGymContext();
        public static MyGymContext DB
        {
            get { return db; }
            set { db = value; }
        }
        private Random random = new Random();
        public Random Random
        {
            get { return random; }
            set { random = value; }
        }
        // Usuario
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Historial> Historial { get; set; }
        public DbSet<PreferenciaTiempoComida> PreferenciaTiempoComida { get; set; }
        // Dieta
        public DbSet<Alimento> Alimento { get; set; }
        public DbSet<Dieta> Dieta { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
        public DbSet<Recomendacion> Recomendacion { get; set; }
        public DbSet<SeConforma> SeConforma { get; set; }
        public DbSet<SeRecomienda> SeRecomienda { get; set; }
        public DbSet<TiempoDeComida> TiempoDeComida { get; set; }
        public DbSet<Tiene> Tiene { get; set; }
        // Rutinas
        public DbSet<Rutina> Rutina { get; set; }
        public DbSet<Ejercicio> Ejercicio { get; set; }
        public DbSet<Actividad> Actividad { get; set; }

        public MyGymContext()
            : base("MyGymDB")
        {
            //Database.SetInitializer<MyGymContext>(new MyGymInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    public class MyGymInitializer : DropCreateDatabaseAlways<MyGymContext>
    {
        protected override void Seed(MyGymContext context)
        {
            #region Tiempos de Comida
            context.TiempoDeComida.Add(new TiempoDeComida()
            {
                Nombre = TiempoComida.Desayuno,
                HoraInicio = new TimeSpan(6, 0, 0),
                HoraFinal = new TimeSpan(10, 0, 0)
            });
            context.TiempoDeComida.Add(new TiempoDeComida()
            {
                Nombre = TiempoComida.Merienda,
                HoraInicio = new TimeSpan(10, 0, 0),
                HoraFinal = new TimeSpan(12, 0, 0)
            });
            context.TiempoDeComida.Add(new TiempoDeComida()
            {
                Nombre = TiempoComida.Almuerzo,
                HoraInicio = new TimeSpan(12, 0, 0),
                HoraFinal = new TimeSpan(14, 0, 0)
            });
            context.TiempoDeComida.Add(new TiempoDeComida()
            {
                Nombre = TiempoComida.Merienda,
                HoraInicio = new TimeSpan(14, 0, 0),
                HoraFinal = new TimeSpan(18, 0, 0)
            });
            context.TiempoDeComida.Add(new TiempoDeComida()
            {
                Nombre = TiempoComida.Cena,
                HoraInicio = new TimeSpan(18, 0, 0),
                HoraFinal = new TimeSpan(20, 0, 0)
            });
            context.SaveChanges();
            #endregion
            #region Usuario
            var user = context.Usuario.Add(new Usuario()
            {
                ComplexionFisica = ComplexionFisica.Mediana,
                Email = "Juan@hotmail.com",
                Estatura = 1.62,
                FechaNacimiento = new DateTime(1991, 1, 25),
                Materno = "Fernandez",
                Nick = "Juan",
                Nombre = "Juan Carlos",
                Password = "123456",
                Paterno = "Cortez",
                Peso = 55,
                Sexo = Common.Enum.Sexo.Masculino,
            });
            context.SaveChanges();
            #endregion
            #region Grupos
            context.Grupo.Add(new Grupo()
            {
                Nombre = "Cereales",
                RecomendacionMaxima = 10,
                RecomendacionMinima = 5
            });
            context.Grupo.Add(new Grupo()
            {
                Nombre = "Lacteos",
                RecomendacionMaxima = 10,
                RecomendacionMinima = 5
            });
            context.Grupo.Add(new Grupo()
            {
                Nombre = "Frutas",
                RecomendacionMaxima = 10,
                RecomendacionMinima = 5
            });
            context.Grupo.Add(new Grupo()
            {
                Nombre = "Azucares y Dulces",
                RecomendacionMaxima = 10,
                RecomendacionMinima = 5
            });
            context.SaveChanges();
            #endregion
            #region Alimentos
            context.Alimento.Add(new Alimento()
            {
                Nombre = "Arroz",
                Calorias = 362,
                Grasas = 0.6,
                GrupoID = 1,
                HidratosDeCarbono = 87.6,
                Proteinas = 7
            });
            context.Alimento.Add(new Alimento()
            {
                Nombre = "Leche Entera",
                Calorias = 63,
                Grasas = 3.7,
                GrupoID = 2,
                HidratosDeCarbono = 4.6,
                Proteinas = 3.2
            });
            context.Alimento.Add(new Alimento()
            {
                Nombre = "Limon",
                Calorias = 14,
                Grasas = 0,
                GrupoID = 3,
                HidratosDeCarbono = 3.2,
                Proteinas = 0.6
            });
            context.Alimento.Add(new Alimento()
            {
                Nombre = "Azucar Blanca",
                Calorias = 385,
                Grasas = 0,
                GrupoID = 4,
                HidratosDeCarbono = 99.5,
                Proteinas = 0
            });
            context.SaveChanges();
            #endregion
            #region Recomendaciones
            context.Recomendacion.Add(new Recomendacion()
            {
                Nombre = "Arroz con Leche",
                ImageUrl = "http://www.dietas.net/images/articulos/articulos-v2/arroz-con-leche.jpg",
                Preparacion = "Calentar la leche en una olla con la cáscara de limón, la canela en rama y el azúcar.\nCuando empiece a hervir, añadiremos el arroz y lo dejaremos cocer durante 20 minutos aproximadamente.\nPasados los 20 minutos retiraremos la cáscara del limón y la canela en rama y ya podremos servir el postre en fuentes individuales.\nEste postre se puede servir tanto en frío como en caliente."
            });
            context.SaveChanges();
            context.SeRecomienda.Add(new SeRecomienda()
            {
                RecomendacionID = 1,
                TiempoDeComidaID = 1
            });
            context.SeRecomienda.Add(new SeRecomienda()
            {
                RecomendacionID = 1,
                TiempoDeComidaID = 2
            });
            context.SaveChanges();
            context.SeConforma.Add(new SeConforma()
            {
                AlimentoID = 1,
                Cantidad = 220,
                RecomendacionID = 1
            });
            context.SeConforma.Add(new SeConforma()
            {
                AlimentoID = 2,
                Cantidad = 200,
                RecomendacionID = 1
            });
            context.SeConforma.Add(new SeConforma()
            {
                AlimentoID = 3,
                Cantidad = 10,
                RecomendacionID = 1
            });
            context.SeConforma.Add(new SeConforma()
            {
                AlimentoID = 4,
                Cantidad = 20,
                RecomendacionID = 1
            });
            context.SaveChanges();
            #endregion
            #region Dieta
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Lunes,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Martes,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Miercoles,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Jueves,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Viernes,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Sabado,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.Dieta.Add(new Dieta()
            {
                UsuarioID = 1,
                Dia = Dia.Domingo,
                Calorias = 300,
                Grasas = 100,
                Proteinas = 200,
                HidratosCarbono = 200
            });
            context.SaveChanges();
            context.Tiene.Add(new Tiene()
            {
                DietaID = 1,
                RecomendacionID = 1
            });
            context.Tiene.Add(new Tiene()
            {
                DietaID = 2,
                RecomendacionID = 1
            });
            context.Tiene.Add(new Tiene()
            {
                DietaID = 3,
                RecomendacionID = 1
            });
            context.Tiene.Add(new Tiene()
            {
                DietaID = 4,
                RecomendacionID = 1
            });
            context.Tiene.Add(new Tiene()
            {
                DietaID = 5,
                RecomendacionID = 1
            });
            context.Tiene.Add(new Tiene()
            {
                DietaID = 6,
                RecomendacionID = 1
            });
            context.Tiene.Add(new Tiene()
            {
                DietaID = 7,
                RecomendacionID = 1
            });
            context.SaveChanges();
            #endregion
            //#region Ejercicios
            //#region Gimnasia
            //{
            //    context.Ejercicio.Add(new Ejercicio()
            //    {
            //        Tipo = TipoEjercicio.Gimnastics,
            //        Nombre = "Sentadillas libres"
            //    });
            //    context.SaveChanges();
            //}
            //#endregion
            //#endregion
            base.Seed(context);
        }
    }
}
