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
        public bool Update(int recid, Recomendacion rec)
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
                foreach (var item in tiemposComida)
                {
                    MyGymContext.DB.SeRecomienda.Add(new SeRecomienda() { 
                        RecomendacionID = recomendation.RecomendacionID,
                        TiempoDeComidaID = item
                    });
                    MyGymContext.DB.SaveChanges();
                }
                foreach (var item in alimentos)
                {
                    SeConforma alimento = MyGymContext.DB.SeConforma.Add(new SeConforma() { 
                        AlimentoID = item.AlimentoID,
                        Cantidad = item.Cantidad,
                        RecomendacionID = recomendation.RecomendacionID
                    });
                    MyGymContext.DB.SaveChanges();
                    recomendation.Proteinas += alimento.Alimento.Proteinas * (item.Cantidad / 100);
                    recomendation.Calorias += alimento.Alimento.Calorias * (item.Cantidad / 100);
                    recomendation.Grasas += alimento.Alimento.Grasas * (item.Cantidad / 100);
                    recomendation.HidratosDeCarbono += alimento.Alimento.HidratosDeCarbono * (item.Cantidad / 100);
                }
                MyGymContext.DB.SaveChanges();
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
            if (recomendationid == null)
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
    }
}