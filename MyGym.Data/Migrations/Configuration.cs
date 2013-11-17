namespace MyGym.Data.Migrations
{
    using MyGym.Common.Enum;
    using MyGym.Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyGym.Data.MyGymContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyGym.Data.MyGymContext context)
        {
            context.Usuario.Add(new Usuario()
            {
                ComplexionFisica = ComplexionFisica.Mediana,
                Email = "yerson_kira@hotmail.com",
                Estatura = 1.75,
                FechaNacimiento = new DateTime(1991, 1, 25),
                Materno = "Pariapaza",
                Nick = "Yerson",
                Nombre = "Yerson Marvin",
                Password = "123456",
                Paterno = "Copa",
                Peso = 59,
                Sexo = Common.Enum.Sexo.Masculino,
            });
            context.SaveChanges();
        }
    }
}
