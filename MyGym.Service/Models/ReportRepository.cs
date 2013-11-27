using MyGym.Common;
using MyGym.Data;
using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models.APIHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGym.Service.Models
{
    public class ReportRepository
    {
        public object GetUserRecord(int userid)
        {
            IEnumerable<UserRecord> datas = default(IEnumerable<UserRecord>);
            datas = MyGymContext.DB.Historial.Where(
                            item => item.UsuarioID == userid).Select(e =>
                                new UserRecord()
                                {
                                    Weight = e.Peso,
                                    Heigth = e.Estatura,
                                    Date = e.Fecha,
                                    NutritionalStatus = e.EstadoNutricional
                                });
            if (datas.Count() > 0)
            {
                return APIFunctions.SuccessResult(datas, JsonMessage.Success);
            }
            return APIFunctions.ErrorResult(string.Format(JsonMessage.NotFound, "Reportes"));
        }
        public object GetIntake(int userid, string filter = "week")
        {
            filter = filter.ToLower();
            var data = default(IEnumerable<UserReport>);
            switch (filter)
            {
                case "week":
                    {
                        data = from consumo in MyGymContext.DB.Consumo.ToList().Take(35)
                               group consumo by consumo.Fecha into g
                               select new UserReport()
                               {
                                   Calories = g.Sum(item => item.Recomendacion.Calorias),
                                   Carbohydrates = g.Sum(item => item.Recomendacion.HidratosDeCarbono),
                                   Grease = g.Sum(item => item.Recomendacion.Grasas),
                                   Protein = g.Sum(item => item.Recomendacion.Proteinas),
                                   Date = g.Key
                               };
                        data = data.Take(7);
                    }
                    break;
                case "month":
                    {
                        

                    }
                    break;
                case "year":
                    {

                    }
                    break;
            }
            return APIFunctions.SuccessResult(data, JsonMessage.Success);
        }
    }
}