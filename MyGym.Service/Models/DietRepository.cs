﻿using MyGym.Common;
using MyGym.Common.Enum;
using MyGym.Data;
using MyGym.Data.Entities;
using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models.APIHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MyGym.Service.Models
{
    public class DietRepository
    {
        public static Dia[] dias = { Dia.Lunes, Dia.Martes, Dia.Miercoles, Dia.Jueves, Dia.Viernes, Dia.Sabado, Dia.Domingo };
        
        public object GetUserDiet(int userid, string day)
        {
            Dia dia = (Dia)Enum.Parse(typeof(Dia), day, true);
            var diet = MyGymContext.DB.Dieta.ToList().FirstOrDefault(item => item.UsuarioID == userid & item.Dia == dia);
            if (diet == null)
            {
                return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Dieta"));
            }
            var mealtimes = MyGymContext.DB.PreferenciaTiempoComida.ToList().Where(item => item.UsuarioID == userid).Select(item => item.TiempoDeComida.Nombre);
            List<UserDiet> result = new List<UserDiet>();
            foreach (var item in mealtimes)
            {
                var recomendation = GetByMealTime(item, diet.DietaID).ToList();
                foreach (var rec in recomendation.Take(3))
                {
                    var search = result.FirstOrDefault(r => r.RecomendationID == rec.RecomendacionID);
                    if (search == null)
                    {
                        UserDiet userdiet = new UserDiet()
                        {
                            DietID = diet.DietaID,
                            ImageURL = rec.ImageUrl,
                            Name = rec.Nombre,
                            RecomendationID = rec.RecomendacionID
                        };
                        userdiet.MealTime = rec.SeRecomiendaEn.ToList().Select(tc => tc.TiempoDeComida.Nombre.ToString());
                        result.Add(userdiet);
                    }
                }
            }
            return APIFunctions.SuccessResult(result, JsonMessage.Success);
        }
        public object GenerateDiet(int userid, UserInformation userdata)
        {
            DateTime lastupdate = GetLastUpdate(userid);
            if (DateTime.Now.Month > lastupdate.Month)
            {
                var user = MyGymContext.DB.Usuario.Find(userid);
                user.Estatura = userdata.Height;
                user.FechaNacimiento = userdata.DateOfBirth;
                user.Peso = userdata.Weight;
                //MyGymContext.DB.Entry<Usuario>(user).CurrentValues.SetValues(APIFunctions.UserToUsuario(userdata));
                //MyGymContext.DB.Entry<Usuario>(user).State = System.Data.EntityState.Modified;
                MyGymContext.DB.SaveChanges();
                this.CreateDiet(user.UsuarioID);
                return APIFunctions.SuccessResult(new object(), JsonMessage.Success);
            }
            return APIFunctions.ErrorResult(JsonMessage.CannotChangeProperty);
        }
        public IEnumerable<Recomendacion> GetByMealTime(TiempoComida mealtime, int dietid)
        { 
            var allrecomendations = MyGymContext.DB.Tiene.ToList().Where(item => item.DietaID == dietid);
            if (allrecomendations.Count() > 0)
            {
                var bymealtime = allrecomendations.Where(item => item.Recomendacion.SeRecomiendaEn.Select(tc => tc.TiempoDeComida.Nombre).Contains(mealtime)).ToList();
                var size = bymealtime.Count();
                if (size < 4)
                {
                    return bymealtime.Select(rec => rec.Recomendacion);
                }
                int startindex = MyGymContext.DB.Random.Next(size);
                List<Recomendacion> result = new List<Recomendacion>();
                for (int i = startindex; i < startindex + 4; i++)
                {
                    result.Add(bymealtime[i % size].Recomendacion);
                }
                return result.AsEnumerable();
            }
            return default(IEnumerable<Recomendacion>);
        }
        public void CreateDiet(int userid)
        {
            // borramos dieta anterior
            //this.DeleteDiet(userid);
            Usuario user = MyGymContext.DB.Usuario.Find(userid);
            // Obtenemos los tiempos de comida
            var tcuser = MyGymContext.DB.PreferenciaTiempoComida.Where(item => item.UsuarioID == user.UsuarioID).Select(s => s.TiempoDeComidaID);
            int tiemposdecomida = tcuser.Count(); 
            // Indice de masa corporal del usuario
            double userIMC = GetIMC(user.Peso, user.Estatura);
            // Estdado nutricional del usuario
            EstadoNutricional userstatus = GetNutritionalStatus(userIMC);
            // Porcentaje de aumento o disminucion de hidratos de carbono y grasas
            double porcentaje = 0;
            // Gasto energetico en reposo kilocalorias/dia
            double GER = 0;
            // Edad del usuario
            int userage = DateTime.Now.Year - user.FechaNacimiento.Year;

            if (userstatus != EstadoNutricional.Normal)
            {
                // Peso minimo recomendable 
                double minweigth = 18.5 * Math.Pow(user.Estatura, 2);
                // Peso maximo optimo
                double maxweigth = 24.99 * Math.Pow(user.Estatura, 2);
                if (user.Peso < minweigth)
                {
                    porcentaje = userIMC / 18.5;
                    if (porcentaje > 1)
                    {
                        porcentaje = 1;
                    }
                    GER = GetGER(minweigth, userage, user.Sexo);
                }
                else if (user.Peso > maxweigth)
                {
                    porcentaje = userIMC / 40;
                    if (porcentaje > 1)
                    {
                        porcentaje = 1;
                    }
                    porcentaje *= -1;
                    GER = GetGER(maxweigth, userage, user.Sexo);
                }
            }
            else
            {
                GER = GetGER(user.Peso, userage, user.Sexo);
            }

            double kcaloriasbyweek, proteinasbyweek, grasasbyweek, hidcarbyweek;
            kcaloriasbyweek = 0;
            proteinasbyweek = 0;
            grasasbyweek = 0;
            hidcarbyweek = 0;
            // calorias quemadas por dia en kilocalorias
            double[] caloriesbyday = { 100, 200, 300, 400, 500, 600, 700 };

            double activityfactor = GetActivityFactor(user.Nivel, user.Sexo);

            for (int i = 0; i < dias.Length; i++)
            {
                // Kilocalorias por dia
                double kcal = (caloriesbyday[i] + GER) * activityfactor;
                // Proteinas en gramos
                double proteinas = (0.15 * kcal) / 4;
                // Grasas en gramos
                double grasas = (0.25 * kcal) / 4;
                grasas += grasas * porcentaje;
                // Hidratos de carbono en gramos
                double hidratosdecarbono = (0.6 * kcal) / 9;
                hidratosdecarbono += hidratosdecarbono * porcentaje;
                // Recomendaciones en base a las proteinas, grasas e hidratos de carbono necesarias
                var recomendations = GetRecomendaciones(
                                        kcal / tiemposdecomida,
                                        proteinas / tiemposdecomida,
                                        hidratosdecarbono / tiemposdecomida,
                                        grasas / tiemposdecomida,
                                        0.2
                                    ).ToList();
                // Adicionar la nueva dieta del dia
                var dia = dias[i];
                var newdiet = MyGymContext.DB.Dieta.ToList().FirstOrDefault(item => item.UsuarioID == user.UsuarioID && item.Dia == dia);
                newdiet.Calorias = kcal;
                newdiet.Grasas = grasas;
                newdiet.HidratosCarbono = hidratosdecarbono;
                newdiet.Proteinas = proteinas;
                //var newdiet = MyGymContext.DB.Dieta.Add(new Dieta() 
                //{ 
                //    Calorias = kcal,
                //    Dia = dias[i],
                //    Grasas = grasas,
                //    HidratosCarbono = hidratosdecarbono,
                //    Proteinas = proteinas,
                //    UsuarioID = user.UsuarioID
                //});
                MyGymContext.DB.SaveChanges();
                // Eliminar las anteriores recomendaciones
                var rec = MyGymContext.DB.Tiene.ToList().Where(item => item.DietaID == newdiet.DietaID);
                foreach (var item in rec)
                {
                    MyGymContext.DB.Tiene.Remove(item);
                    MyGymContext.DB.SaveChanges();
                }
                // Adicionar las recomendaciones a la dieta
                foreach (var item in recomendations)
                {
                    var tcrec = item.SeRecomiendaEn.Select(s => s.TiempoDeComidaID);
                    if ((tcrec.Intersect(tcuser)).Count() > 0 & ValidateRecomendation(item, newdiet, user, userstatus, tcuser))
                    {
                        MyGymContext.DB.Tiene.Add(new Tiene() 
                        { 
                            DietaID = newdiet.DietaID,
                            RecomendacionID = item.RecomendacionID
                        });
                        MyGymContext.DB.SaveChanges();
                    }
                }
                // Promedio semanal
                kcaloriasbyweek += kcal;
                proteinasbyweek += proteinas;
                hidcarbyweek += hidratosdecarbono;
                grasasbyweek += grasas;
            }
            // Adicionamos al historial
            MyGymContext.DB.Historial.Add(new Historial() 
            { 
                Estatura = user.Estatura,
                Calorias = kcaloriasbyweek / 7,
                EstadoNutricional = userstatus,
                Fecha = DateTime.Now, 
                Grasas = grasasbyweek / 7,
                HidratosCarbono = hidcarbyweek / 7,
                IMC = userIMC,
                Peso = user.Peso,
                Proteinas = proteinasbyweek / 7,
                UsuarioID = user.UsuarioID
            });
            MyGymContext.DB.SaveChanges();
        }
        public bool ValidateRecomendation(Recomendacion recomendation, Dieta diet, Usuario user, EstadoNutricional userstatus, IEnumerable<int> mealtimes)
        {
            int tc = mealtimes.Count();
            double error = 0.4;
            double maxcalorias = (diet.Calorias / tc) + ((diet.Calorias / tc) * error);
            double mincalorias = (diet.Calorias / tc) - ((diet.Calorias / tc) * error);
            double maxgrasas = (diet.Grasas / tc) + ((diet.Grasas / tc) * error);
            double mingrasas = (diet.Grasas / tc) - ((diet.Grasas / tc) * error);
            if (userstatus == EstadoNutricional.Infrapeso | userstatus == EstadoNutricional.DelgadezSevera | userstatus == EstadoNutricional.DelgadezModerada | userstatus == EstadoNutricional.DelgadezNoMuyPronunciada)
            {
                if (recomendation.Calorias < mincalorias | recomendation.Grasas < mingrasas)
                {
                    return false;
                }
            }
            else if (userstatus == EstadoNutricional.Sobrepeso | userstatus == EstadoNutricional.Preobeso | userstatus == EstadoNutricional.Obeso | userstatus == EstadoNutricional.ObesoTipoI | userstatus == EstadoNutricional.ObesoTipoII | userstatus == EstadoNutricional.ObesoTipoIII)
            {
                if (recomendation.Calorias > maxcalorias | recomendation.Grasas > maxgrasas)
                {
                    return false;
                }
            }
            else
            {
                // Evitar exceso de grasas
                if (recomendation.Grasas > maxgrasas)
                {
                    return false;
                }
            }
            return true;
        }
        public object ConsumeRecomendation(int userid, int recomendationid, string date, string mealtime)
        {
            try
            {

                var tiempocomida = (TiempoComida)Enum.Parse(typeof(TiempoComida), mealtime, true);
                var fecha = DateTime.Parse(date);
                var user = MyGymContext.DB.Usuario.Find(userid);
                var recomendation = MyGymContext.DB.Recomendacion.Find(recomendationid);
                if (user == null || recomendation == null)
                {
                    return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Usuario o Recomendacion"));
                }
                MyGymContext.DB.Consumo.Add(new Consumo() { 
                    UsuarioID = user.UsuarioID,
                    RecomendacionID = recomendation.RecomendacionID,
                    Fecha = fecha,
                    TiempoComida = tiempocomida
                });
                MyGymContext.DB.SaveChanges();
                return APIFunctions.SuccessResult(new object(), JsonMessage.Success);
            }
            catch (Exception ex)
            {
                return APIFunctions.ErrorResult(ex.Message);
            }
        }
        private double GetActivityFactor(Nivel level, Sexo sex)
        {
            switch (level)
            {
                case Nivel.MuyLeve:
                    return 1.3;
                case Nivel.Leve:
                    return sex == Sexo.Masculino ? 1.6 : 1.5;
                case Nivel.Moderado:
                    return sex == Sexo.Masculino ? 1.7 : 1.6;
                case Nivel.Intenso:
                    return sex == Sexo.Masculino ? 2.1 : 1.9;
                default:
                    return sex == Sexo.Masculino ? 2.4 : 2.2;
            }
        }
        private IEnumerable<Recomendacion> GetRecomendaciones(double calorias, double proteinas, double hidratoscarbono, double grasas, double error)
        {
            double proteinasmin, proteinasmax, grasasmin, grasasmax, hidcarmin, hidcarmax, caloriasmin, caloriasmax;
            proteinasmin = proteinas - (proteinas * error);
            proteinasmax = proteinas + (proteinas * error);
            grasasmin = grasas - (grasas * error);
            grasasmax = grasas + (grasas * error);
            hidcarmin = hidratoscarbono - (hidratoscarbono * error);
            hidcarmax = hidratoscarbono + (hidratoscarbono * error);
            caloriasmin = calorias - (calorias * error);
            caloriasmax = calorias + (calorias * error);
            return MyGymContext.DB.Recomendacion.ToList().Where(item =>
            {
                return (item.Calorias >= caloriasmin & item.Calorias <= caloriasmax) |
                (item.Proteinas >= proteinasmin & item.Proteinas <= proteinasmax) |
                (item.Grasas >= grasasmin & item.Grasas <= grasasmax) |
                (item.HidratosDeCarbono >= hidcarmin & item.HidratosDeCarbono <= hidcarmax);
            });
        }
        private double GetGER(double peso, int userage, Sexo sexo)
        {
            if (sexo == Common.Enum.Sexo.Masculino)
            {
                if (userage < 3)
                {
                    return (6.90 * peso) - 54;
                }
                else if (userage < 10)
                {
                    return (22.7 * peso) + 495;
                }
                else if (userage < 18)
                {
                    return (17.5 * peso) + 651;
                }
                else if (userage < 30)
                {
                    return (15.3 * peso) + 679;
                }
                else if (userage < 60)
                {
                    return (11.6 * peso) + 879;
                }
                else
                {
                    return (13.5 * peso) + 487;
                }
            }
            else
            {
                if (userage < 3)
                {
                    return (61.0 * peso) - 51;
                }
                else if (userage < 10)
                {
                    return (22.5 * peso) + 499;
                }
                else if (userage < 18)
                {
                    return (12.2 * peso) + 746;
                }
                else if (userage < 30)
                {
                    return (14.7 * peso) + 496;
                }
                else if (userage < 60)
                {
                    return (14.7 * peso) + 746;
                }
                else
                {
                    return (10.5 * peso) + 596;
                }
            }
        }
        private double GetIMC(double peso, double altura)
        {
            return peso / Math.Pow(altura, 2);
        }
        private EstadoNutricional GetNutritionalStatus(double imc)
        {
            if (imc < 15.99)
            {
                return EstadoNutricional.Infrapeso;
            }
            else if (imc < 16)
            {
                return EstadoNutricional.DelgadezSevera;
            }
            else if (imc < 16.99)
            {
                return EstadoNutricional.DelgadezModerada;
            }
            else if (imc < 18.49)
            {
                return EstadoNutricional.DelgadezNoMuyPronunciada;
            }
            else if (imc < 24.99)
            {
                return EstadoNutricional.Normal;
            }
            else if (imc <= 25)
            {
                return EstadoNutricional.Sobrepeso;
            }
            else if (imc < 29.99)
            {
                return EstadoNutricional.Preobeso;
            }
            else if (imc <= 30)
            {
                return EstadoNutricional.Obeso;
            }
            else if (imc < 34.99)
            {
                return EstadoNutricional.ObesoTipoI;
            }
            else if (imc < 39.99)
            {
                return EstadoNutricional.ObesoTipoII;
            }
            else
            {
                return EstadoNutricional.ObesoTipoIII;
            }
        }
        public void DeleteDiet(int userid)
        {
            Usuario user = MyGymContext.DB.Usuario.Find(userid);

            foreach (var item in user.Dieta)
            {
                var dieta = MyGymContext.DB.Tiene.ToList().FirstOrDefault(e => e.DietaID == item.DietaID);
                MyGymContext.DB.Tiene.Remove(dieta);
                MyGymContext.DB.SaveChanges();
            }
            user.Dieta = new HashSet<Dieta>();
            MyGymContext.DB.SaveChanges();
        }
        public DateTime GetLastUpdate(int userid)
        {
            var records = MyGymContext.DB.Historial.Where(item => item.UsuarioID == userid).OrderBy(item => item.Fecha).FirstOrDefault();
            if (records == null)
            {
                return default(DateTime);
            }
            return records.Fecha;
        }
    }
}