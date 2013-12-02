using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGym.Service.Controllers.API
{
    public class ReportController : Controller
    {
        [HttpGet]
        [APIErrorHandler]
        public JsonResult Get(int userid)
        {
            var result = new ReportRepository().GetUserRecord(userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [APIErrorHandler]
        public JsonResult GetConsumo(int userid, string filter)
        {
            var result = new ReportRepository().GetIntake(userid, filter);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
