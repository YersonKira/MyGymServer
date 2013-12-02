using MyGym.Data.Entities;
using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGym.Service.Controllers.API
{
    public class ActivityController : Controller
    {
        [HttpPost]
        [APIErrorHandler]
        public JsonResult activityCompleted(string activityData)
        {
            var result = new ActivityRepository().ActivityCompleted(JsonConvert.DeserializeObject<Actividad>(activityData));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
