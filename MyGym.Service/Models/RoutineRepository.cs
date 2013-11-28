using MyGym.Common;
using MyGym.Common.Enum;
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
    public class RoutineRepository
    {
        public Random random { get; set; }

        public object GenerateRoutine(int UsuarioId, bool mode)
        {
            var user = MyGymContext.DB.Usuario.Find(UsuarioId);
            if (user == null)
            {
                return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Usuario"));
            }
            Rutina rutina = new Rutina();
            rutina.UsuarioID = user.UsuarioID;
            MyGymContext.DB.Rutina.Add(rutina);
            MyGymContext.DB.SaveChanges();
            ExerciseRepository methods = new ExerciseRepository();
            if (mode)
            {
                List<Actividad> activities = new List<Actividad>();
                List<int> serie = Serie(2, 3, 3).ToList();
                for (int i = 0; i < serie.Count; i++)
                {
                    if (i % 4 == 3)
                        activities.Insert(i, new Actividad() { EjercicioID = 1, Fecha = DateTime.Now.AddDays(i), RutinaID = rutina.RutinaID });
                    else
                    {
                        activities[i].Fecha = DateTime.Now.AddDays(i);
                        activities[i].RutinaID = rutina.RutinaID;
                    }
                    MyGymContext.DB.Actividad.Add(activities[i]);
                    MyGymContext.DB.SaveChanges();
                }
                return APIFunctions.SuccessResult(toSerializable(activities), JsonMessage.Success);
            }
            else
            {
                List<Actividad> activities = new List<Actividad>();
                for (int i = 0; i < 21; i++)
                {
                    if (i % 7 == 5 || i % 7 == 6)
                        activities.Insert(i, new Actividad() { EjercicioID = 1, Fecha = DateTime.Now.AddDays(i), RutinaID = rutina.RutinaID });
                    else
                    {
                        activities[i].Fecha = DateTime.Now.AddDays(i);
                        activities[i].RutinaID = rutina.RutinaID;
                    }
                    MyGymContext.DB.Actividad.Add(activities[i]);
                    MyGymContext.DB.SaveChanges();
                }
                return APIFunctions.SuccessResult(toSerializable(activities), JsonMessage.Success);
            }

        }
        private object toSerializable(List<Actividad> activities)
        {
            return activities.Select(p => new { Date = p.Fecha, ExerciseID = p.EjercicioID });
        }

        public IEnumerable<int> Serie(int repeticiones, int limite, int giros)
        {
            List<int> resultado = new List<int>();
            for (int i = 0; i < giros; i++)
            {
                for (int j = 0; j < repeticiones; j++)
                {
                    for (int k = 0; k < limite; k++)
                    {
                        resultado.Add(i % limite);
                    }
                }
            }
            return resultado;
        }
    }
}