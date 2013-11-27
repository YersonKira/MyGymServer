using MyGym.Common;
using MyGym.Data;
using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models;
using MyGym.Service.Models.APIHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MyGym.Service.Controllers.API
{
    public class DietController : Controller
    {
        
        /// <summary>
        /// Retorna las recomendaciones de la dieta de un usuario en base a un dia de la semana
        /// </summary>
        /// <param name="userid">Identificador unico del usuario</param>
        /// <param name="day">Dia de la semana de tipo <typeparamref name="MyGym.Common.Enum.Dia"/></param>
        /// <returns>Retorna una coleccion de objetos de tipo <paramref name="MyGym.Common.UserDiet"/></returns>
        [HttpGet]
        [APIErrorHandler]
        public JsonResult GetDiet(int userid, string day)
        {
            var result = new DietRepository().GetUserDiet(userid, day);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Actualiza la dieta de un usuario al cambiar datos como el peso, la estatura o la fecha de nacimiento
        /// </summary>
        /// <param name="userid">Idenfificador del usuario</param>
        /// <param name="userdatas">Representacion en Json del objeto MyGym.Common.UserInformation con los nuevos datos de usuario</param>
        /// <returns>Retorna un objeto vacio</returns>
        [HttpPost]
        [APIErrorHandler]
        public JsonResult UpdateDiet(int userid, string userdatas)
        {
            var user = JsonConvert.DeserializeObject<UserInformation>(userdatas);
            var result = new DietRepository().GenerateDiet(userid, user);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Retorna los detalles de una recomendacion
        /// </summary>
        /// <param name="recomendationid">identificador unico de la recomendacion</param>
        /// <returns>Retorna un objeto del tipo <paramref name="MyGym.Common.UserRecomendation"/></returns>
        [HttpGet]
        [APIErrorHandler]
        public JsonResult GetRecomendation(int recomendationid)
        {
            var result = new RecomendationRepository().GetUserRecomendation(recomendationid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [APIErrorHandler]
        public JsonResult ConsumeRecomendation(int userid, int recomendationid, string date, string mealtime)
        {
            var result = new DietRepository().ConsumeRecomendation(userid, recomendationid, date, mealtime);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
