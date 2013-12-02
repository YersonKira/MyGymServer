using MyGym.Common;
using MyGym.Data;
using MyGym.Data.Entities;
using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models.APIHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGym.Service.Models
{
    public class RecomendationRepository
    {
        public Recomendacion Get(int recid)
        {
            return MyGymContext.DB.Recomendacion.Find(recid);
        }
        public bool Delete(int recid)
        {
            Recomendacion recomendation = MyGymContext.DB.Recomendacion.Find(recid);
            if (recomendation == null)
            {
                return false;
            }
            MyGymContext.DB.Recomendacion.Remove(recomendation);
            MyGymContext.DB.SaveChanges();
            return true;
        }
        public bool Update(int recid, Recomendacion rec, IEnumerable<int> tiemposComida, IEnumerable<SeConforma> alimentos)
        {
            Recomendacion recomendation = MyGymContext.DB.Recomendacion.Find(recid);
            if (recomendation == null)
            {
                return false;
            }
            try
            {
                recomendation.Calorias = rec.Calorias;
                recomendation.Grasas = rec.Grasas;
                recomendation.HidratosDeCarbono = rec.HidratosDeCarbono;
                recomendation.Nombre = rec.Nombre;
                recomendation.Preparacion = rec.Preparacion;
                recomendation.Proteinas = rec.Proteinas;
                recomendation.ImageUrl = rec.ImageUrl;
                // Eliminar anteriores tiempos de comida
                var tc = recomendation.SeRecomiendaEn.ToList();
                foreach (var item in tc)
                {
                    MyGymContext.DB.SeRecomienda.Remove(item);
                }
                MyGymContext.DB.SaveChanges();
                // Eliminar alimentos anteriores
                var foods = recomendation.SeConforma.ToList();
                foreach (var item in foods)
                {
                    MyGymContext.DB.SeConforma.Remove(item);
                }
                MyGymContext.DB.SaveChanges();
                AppendRecomendation(recomendation.RecomendacionID, tiemposComida, alimentos);
                MyGymContext.DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Add(Recomendacion rec, IEnumerable<int> tiemposComida, string urlimage, IEnumerable<SeConforma> alimentos)
        {
            try
            {
                var recomendation = MyGymContext.DB.Recomendacion.Add(rec);
                MyGymContext.DB.SaveChanges();
                AppendRecomendation(recomendation.RecomendacionID, tiemposComida, alimentos);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object GetUserRecomendation(int recomendationid)
        {
            Recomendacion recomendation = Get(recomendationid);
            if (recomendation == null)
            {
                return APIFunctions.ErrorResult(JsonMessage.NotFound);
            }
            UserRecomendation result = new UserRecomendation() { 
                Name = recomendation.Nombre,
                Preparation = recomendation.Preparacion,
                Ingredients = recomendation.SeConforma.Select(item => new UserFood(){
                    Name = item.Alimento.Nombre,
                    Amount = item.Cantidad,
                    Calories = item.Alimento.Calorias,
                    Carbohydrates = item.Alimento.HidratosDeCarbono,
                    Grease = item.Alimento.Grasas,
                    Protein = item.Alimento.Proteinas,
                    Grupo = item.Alimento.Grupo.Nombre
                })
            };
            return APIFunctions.SuccessResult(result, JsonMessage.Success);
        }
        private void AppendRecomendation(int recomendationid, IEnumerable<int> tiemposComida, IEnumerable<SeConforma> alimentos)
        {
            if (tiemposComida != null)
            {
                foreach (var item in tiemposComida)
                {
                    if (item != default(int))
                    {
                        MyGymContext.DB.SeRecomienda.Add(new SeRecomienda()
                        {
                            RecomendacionID = recomendationid,
                            TiempoDeComidaID = item
                        });
                    }
                }
                MyGymContext.DB.SaveChanges();
            }
            foreach (var item in alimentos)
            {
                if (item != null)
                {
                    MyGymContext.DB.SeConforma.Add(new SeConforma()
                    {
                        AlimentoID = item.AlimentoID,
                        Cantidad = item.Cantidad,
                        Descipcion = item.Descipcion,
                        RecomendacionID = recomendationid
                    });
                }
            }
            MyGymContext.DB.SaveChanges();
        }
    }
}