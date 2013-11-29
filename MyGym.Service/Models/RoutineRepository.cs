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
            var query = from x in MyGymContext.DB.Rutina.ToList() where x.UsuarioID == user.UsuarioID select x;
            foreach (var item in query.ToList())
            {
                var exercises = from x in MyGymContext.DB.Actividad.ToList() where x.RutinaID == item.RutinaID orderby x.Fecha select x;
                if (exercises.ToList().Last().Fecha > System.DateTime.Now)
                {
                    return APIFunctions.ErrorResult(string.Format(JsonMessage.Error, "Rutina ya creada"));
                }
            }
            Rutina rutina = new Rutina();
            rutina.UsuarioID = user.UsuarioID;
            MyGymContext.DB.Rutina.Add(rutina);
            MyGymContext.DB.SaveChanges();
            List<Actividad> activities = new List<Actividad>();
            ExerciseRepository methods = new ExerciseRepository();
            if (mode)
            {
                List<int> serie = Serie(2, 3, 3);
                for (int i = 0; i < serie.Count; i++)
                {
                    if (i % 4 == 3)
                        activities.Insert(i, new Actividad() { EjercicioID = -1, Fecha = DateTime.Now.AddDays(i), RutinaID = rutina.RutinaID });
                    else
                    {
                        activities.Insert(i, new Actividad() { EjercicioID = methods.GetRandomExercise(serie[i]).EjercicioID, Fecha = DateTime.Now.AddDays(i), RutinaID = rutina.RutinaID });
                        MyGymContext.DB.Actividad.Add(activities[i]);
                        MyGymContext.DB.SaveChanges();
                    }
                }
                return APIFunctions.SuccessResult(toSerializable(activities), JsonMessage.Success);
            }
            else
            {
                List<int> serie = Serie(3, 3, 3);
                for (int i = 0; i < serie.Count; i++)
                {
                    if (i % 7 == 5 || i % 7 == 6)
                        activities.Insert(i, new Actividad() { EjercicioID = -1, Fecha = DateTime.Now.AddDays(i), RutinaID = rutina.RutinaID });
                    else
                    {
                        activities.Insert(i, new Actividad() { EjercicioID = methods.GetRandomExercise(serie[i]).EjercicioID, Fecha = DateTime.Now.AddDays(i), RutinaID = rutina.RutinaID });
                        MyGymContext.DB.Actividad.Add(activities[i]);
                        MyGymContext.DB.SaveChanges();
                    }
                }
                return APIFunctions.SuccessResult(toSerializable(activities), JsonMessage.Success);
            }

        }
        private object toSerializable(List<Actividad> activities)
        {
            return activities.Select(p => new { Date = p.Fecha, ExerciseID = p.EjercicioID, RoutineID = p.RutinaID });
        }
        public object Get(int routineID)
        {
            var routine = MyGymContext.DB.Rutina.Find(routineID);
            if (routine == null)
            {
                return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Rutina"));
            }
            var activities = from x in MyGymContext.DB.Actividad.ToList() where x.RutinaID == routine.RutinaID select x;
            return APIFunctions.SuccessResult(toSerializable(activities.ToList()), JsonMessage.Success);
        }
        public List<int> Serie(int repeticiones, int limite, int giros)
        {
            List<int> lista = new List<int>();
            for (int i = 0; i < giros; i++)
                for (int j = 0; j < repeticiones; j++)
                    for (int k = 0; k < limite; k++)
                        lista.Add((k + i) % limite);
            return lista;
        }
    }
}