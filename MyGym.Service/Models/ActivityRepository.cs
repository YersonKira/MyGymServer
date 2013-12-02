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
    public class ActivityRepository
    {
        public object ActivityCompleted(Actividad actividad)
        {
            var routine = from x in MyGymContext.DB.Rutina.ToList() where x.RutinaID == actividad.RutinaID select x;
            if (routine == null)
                return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Rutina"));
            var activitys = from x in MyGymContext.DB.Actividad.ToList() where x.RutinaID == routine.ToList()[0].RutinaID select x;
            if (activitys == null)
                return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Actividad"));
            var oldActivity = MyGymContext.DB.Actividad.Find(activitys.ToList()[0]);
            var newActivity = new Actividad()
            {
                ActividadID = oldActivity.ActividadID,
                Completed = true,
                EjercicioID = oldActivity.EjercicioID,
                Fecha = oldActivity.Fecha,
                RutinaID = oldActivity.RutinaID
            };
            MyGymContext.DB.Entry<Actividad>(oldActivity).CurrentValues.SetValues(newActivity);
            MyGymContext.DB.Entry<Actividad>(oldActivity).State = System.Data.EntityState.Modified;
            oldActivity.Completed = true;
            MyGymContext.DB.SaveChanges();
            return APIFunctions.SuccessResult(oldActivity.ActividadID, JsonMessage.Success);
        }
    }
}