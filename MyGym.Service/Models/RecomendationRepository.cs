using MyGym.Data;
using MyGym.Data.Entities;
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
        public int Add(Recomendacion rec)
        {
            try
            {
                MyGymContext.DB.Recomendacion.Add(rec);
                return MyGymContext.DB.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}