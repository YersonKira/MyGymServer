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
        public DbSet<Consumo> Consumo { get; set; }
        public DbSet<Tiene> Tiene { get; set; }
        // Rutinas
        public DbSet<Rutina> Rutina { get; set; }
        public DbSet<Ejercicio> Ejercicio { get; set; }
        public DbSet<Actividad> Actividad { get; set; }

        public MyGymContext()
            : base("MyGymDB")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    public class MyGymInitializer : CreateDatabaseIfNotExists<MyGymContext>
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
                Nombre = TiempoComida.Media_Manana,
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
                Nombre = TiempoComida.Media_Tarde,
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
            //#region Grupos
            //context.Grupo.Add(new Grupo()
            //{
            //    Nombre = "Cereales",
            //    RecomendacionMaxima = 10,
            //    RecomendacionMinima = 5
            //});
            //context.Grupo.Add(new Grupo()
            //{
            //    Nombre = "Lacteos",
            //    RecomendacionMaxima = 10,
            //    RecomendacionMinima = 5
            //});
            //context.Grupo.Add(new Grupo()
            //{
            //    Nombre = "Frutas",
            //    RecomendacionMaxima = 10,
            //    RecomendacionMinima = 5
            //});
            //context.Grupo.Add(new Grupo()
            //{
            //    Nombre = "Azucares y Dulces",
            //    RecomendacionMaxima = 10,
            //    RecomendacionMinima = 5
            //});
            //context.SaveChanges();
            //#endregion
            //#region Alimentos
            //context.Alimento.Add(new Alimento()
            //{
            //    Nombre = "Arroz",
            //    Calorias = 362,
            //    Grasas = 0.6,
            //    GrupoID = 1,
            //    HidratosDeCarbono = 87.6,
            //    Proteinas = 7
            //});
            //context.Alimento.Add(new Alimento()
            //{
            //    Nombre = "Leche Entera",
            //    Calorias = 63,
            //    Grasas = 3.7,
            //    GrupoID = 2,
            //    HidratosDeCarbono = 4.6,
            //    Proteinas = 3.2
            //});
            //context.Alimento.Add(new Alimento()
            //{
            //    Nombre = "Limon",
            //    Calorias = 14,
            //    Grasas = 0,
            //    GrupoID = 3,
            //    HidratosDeCarbono = 3.2,
            //    Proteinas = 0.6
            //});
            //context.Alimento.Add(new Alimento()
            //{
            //    Nombre = "Azucar Blanca",
            //    Calorias = 385,
            //    Grasas = 0,
            //    GrupoID = 4,
            //    HidratosDeCarbono = 99.5,
            //    Proteinas = 0
            //});
            //context.SaveChanges();
            //#endregion
            //#region Recomendaciones
            //context.Recomendacion.Add(new Recomendacion()
            //{
            //    Nombre = "Arroz con Leche",
            //    ImageUrl = "http://www.dietas.net/images/articulos/articulos-v2/arroz-con-leche.jpg",
            //    Preparacion = "Calentar la leche en una olla con la cáscara de limón, la canela en rama y el azúcar.\nCuando empiece a hervir, añadiremos el arroz y lo dejaremos cocer durante 20 minutos aproximadamente.\nPasados los 20 minutos retiraremos la cáscara del limón y la canela en rama y ya podremos servir el postre en fuentes individuales.\nEste postre se puede servir tanto en frío como en caliente."
            //});
            //context.SaveChanges();
            //context.SeRecomienda.Add(new SeRecomienda()
            //{
            //    RecomendacionID = 1,
            //    TiempoDeComidaID = 1
            //});
            //context.SeRecomienda.Add(new SeRecomienda()
            //{
            //    RecomendacionID = 1,
            //    TiempoDeComidaID = 2
            //});
            //context.SaveChanges();
            //context.SeConforma.Add(new SeConforma()
            //{
            //    AlimentoID = 1,
            //    Cantidad = 220,
            //    RecomendacionID = 1
            //});
            //context.SeConforma.Add(new SeConforma()
            //{
            //    AlimentoID = 2,
            //    Cantidad = 200,
            //    RecomendacionID = 1
            //});
            //context.SeConforma.Add(new SeConforma()
            //{
            //    AlimentoID = 3,
            //    Cantidad = 10,
            //    RecomendacionID = 1
            //});
            //context.SeConforma.Add(new SeConforma()
            //{
            //    AlimentoID = 4,
            //    Cantidad = 20,
            //    RecomendacionID = 1
            //});
            //context.SaveChanges();
            //#endregion
            //#region Dieta
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Lunes,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Martes,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Miercoles,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Jueves,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Viernes,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Sabado,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.Dieta.Add(new Dieta()
            //{
            //    UsuarioID = 1,
            //    Dia = Dia.Domingo,
            //    Calorias = 300,
            //    Grasas = 100,
            //    Proteinas = 200,
            //    HidratosCarbono = 200
            //});
            //context.SaveChanges();
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 1,
            //    RecomendacionID = 1
            //});
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 2,
            //    RecomendacionID = 1
            //});
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 3,
            //    RecomendacionID = 1
            //});
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 4,
            //    RecomendacionID = 1
            //});
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 5,
            //    RecomendacionID = 1
            //});
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 6,
            //    RecomendacionID = 1
            //});
            //context.Tiene.Add(new Tiene()
            //{
            //    DietaID = 7,
            //    RecomendacionID = 1
            //});
            //context.SaveChanges();
            //#endregion
            #region Ejercicios
            {
                #region Gimnasia

                context.Ejercicio.Add(new Ejercicio()
                {
                    Tipo = TipoEjercicio.Gimnastics,
                    Nombre = "Sentadillas libres",
                    Repeticiones = 25,
                    Sets = 5,
                    Instruccion = new HashSet<Instruccion>()
                    #region Instrucciones
                    {
                        new Instruccion()
                        {
                            Content="Comenzar con los pies separados siguiendo el ancho de los hombros, con los dedos levemente hacia afuera.",
                            Step=1
                        },
                        new Instruccion()
                        {
                            Content="Mantener la cabeza arriba, con la mirada apenas por encima del paralelo.",
                            Step=2
                        },
                        new Instruccion()
                        {
                            Content="No bajar la mirada; el suelo está en visión periférica únicamente.",
                            Step=3
                        },
                        new Instruccion()
                        {
                            Content="Acentuar el arco normal de la curva lumbar y luego quitar el arco excesivo con los abdominales.",
                            Step=4
                        },
                        new Instruccion()
                        {
                            Content="Mantener el torso medio bien firme.",
                            Step=5
                        },
                        new Instruccion()
                        {
                            Content="Llevar los glúteos hacia atrás y abajo.",
                            Step=6
                        },
                        new Instruccion()
                        {
                            Content="Las rodillas siguen la línea del pie.",
                            Step=7
                        },
                        new Instruccion()
                        {
                            Content="Las rodillas no deben rotar hacia dentro del pie.",
                            Step=8
                        },
                        new Instruccion()
                        {
                            Content="Mantener la mayor presión posible en los talones.",
                            Step=9
                        },
                        new Instruccion()
                        {
                            Content="Separarse de las esferas de la planta del pie.",
                            Step=10
                        },
                        new Instruccion()
                        {
                            Content="Retrasar el movimiento de las rodillas hacia delante tanto como sea posible.",
                            Step=11
                        },
                        new Instruccion()
                        {
                            Content="Levantar y girar los brazos arriba y afuera al descender.",
                            Step=12
                        },
                        new Instruccion()
                        {
                            Content="Mantener el torso alargado.",
                            Step=13
                        },
                        new Instruccion()
                        {
                            Content="Alejar las manos de los glúteos tanto como sea posible.",
                            Step=14
                        },
                        new Instruccion()
                        {
                            Content="De perfil, la oreja no se mueve hacia delante durante las sentadillas, baja directamente.",
                            Step=15
                        },
                        new Instruccion()
                        {
                            Content="No hundirse sino descender con los flexores de la cadera.",
                            Step=16
                        },
                        new Instruccion()
                        {
                            Content="No colapsar la curva lumbar al llegar al piso.",
                            Step=17
                        },
                        new Instruccion()
                        {
                            Content="Detenerse cuando el pliegue de la cadera esté por debajo de la rodilla, romper el paralelo con los muslos.",
                            Step=18
                        },
                        new Instruccion()
                        {
                            Content="Apretar los glúteos y los isquiotibiales y ascender sin inclinarse hacia delante o sin perder el equilibrio.",
                            Step=19
                        },
                        new Instruccion()
                        {
                            Content="Regresar por el mismo camino que al descender.",
                            Step=20
                        },
                        new Instruccion()
                        {
                            Content="Utilizar cada parte de la musculatura que sea posible; no hay parte del cuerpo que no trabaje.",
                            Step=21
                        },
                        new Instruccion()
                        {
                            Content="Al ascender, sin mover los pies, presionar hacia fuera de los pies como si se intentara separar del suelo.",
                            Step=22
                        },
                        new Instruccion()
                        {
                            Content="En lo alto del movimiento, llegar tan alto como sea posible.",
                            Step=23
                        },
                    }
                    #endregion
                });
                context.Ejercicio.Add(new Ejercicio()
                {
                    Tipo = TipoEjercicio.Gimnastics,
                    Nombre = "Dominadas",
                    Repeticiones = 5,
                    Sets = 5,
                    Instruccion = new HashSet<Instruccion>()
                    {
                        new Instruccion()
                        {
                            Content="Cuelguese de la barra fija, con las palmas hacia abajo.",
                            Step=1
                        },
                        new Instruccion()
                        {
                            Content="Suba el peso del cuerpo sin mover los codos hacia adelante, mantega su cuerpo perpendicular al suelo.",
                            Step=2
                        },
                        new Instruccion()
                        {
                            Content="Baje lentamente manteniendo la posicion.",
                            Step=3,
                        }
                    }
                });
                context.SaveChanges();
                #endregion
                #region Cardio
                context.Ejercicio.Add(new Ejercicio()
                {
                    Tipo = TipoEjercicio.Cardio,
                    Nombre = "Correr",
                    Distancia = 0.5,
                    Sets = 1
                });
                context.Ejercicio.Add(new Ejercicio()
                {
                    Tipo = TipoEjercicio.Cardio,
                    Nombre = "Bicicleta",
                    Distancia = 1,
                    Sets = 1
                });
                #endregion
                #region pesas
                context.Ejercicio.Add(new Ejercicio()
                {
                    Tipo = TipoEjercicio.Weights,
                    Nombre = "DeadLift",
                    Repeticiones = 5,
                    Sets = 5,
                    Peso = 50,
                    Instruccion = new HashSet<Instruccion>()
                    {
                        new Instruccion()
                        {
                             Content="Tenga una postura natural con los pies dabajo de la cadera",
                             Step=1
                        },
                        new Instruccion()
                        {
                             Content="Debe agarrar el peso simetricamente, ya sea paralelo, gancho o mixto",
                             Step=2
                        },
                        new Instruccion()
                        {
                             Content="Hombros ligeramente delante de la barra",
                             Step=3
                        },
                        new Instruccion()
                        {
                             Content="La parte interna del los codos deben estar paralelos",
                             Step=4
                        },
                        new Instruccion()
                        {
                             Content="Mantenga el peso en los talones",
                             Step=5
                        },
                        new Instruccion()
                        {
                             Content="El recorrido de la barra hacia arriba y abajo cerca de las piernas",
                             Step=6
                        },
                        new Instruccion()
                        {
                             Content="Mantenga la cabeza hacia adelante",
                             Step=7
                        },
                    }
                });
                context.Ejercicio.Add(new Ejercicio()
                {
                    Tipo = TipoEjercicio.Weights,
                    Nombre = "Press de hombros",
                    Sets = 5,
                    Repeticiones = 5,
                    Peso = 20,
                    Instruccion = new HashSet<Instruccion>()
                    {
                        new Instruccion()
                        {
                            Content="Tomar  la  barra  del soporte o cargarla desde la posición de  rack.  La  barra  descansa  en  los hombros  con  un  agarre  levemente más  ancho  que  el  ancho  de  los hombros.",
                            Step=1
                        },
                        new Instruccion()
                        {
                            Content="Los  codos  están  por debajo  y  al  frente  de  la  barra.  Los pies deben estar aproximadamente en el mismo ancho que los hombros.",
                            Step=2
                        },
                        new Instruccion()
                        {
                            Content="La cabeza está  levemente  inclinada hacia atrás para permitir el paso de la barra. ",
                            Step=3
                        },
                        new Instruccion()
                        {
                            Content="Empuje  la  barra  a  una posición directamente encima de la cabeza.",
                            Step=4
                        },
                        new Instruccion()
                        {
                            Content="Empuje  la  barra  a  una posición directamente encima de la cabeza.",
                            Step=4
                        }
                    }
                });
                #endregion
                context.SaveChanges();
            }
            #endregion
            base.Seed(context);
        }
    }
}
