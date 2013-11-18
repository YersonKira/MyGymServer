using MyGym.Data;
using MyGym.Data.Entities;
using MyGym.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGym.Service.Controllers
{
    public class RecomendationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(MyGymContext.DB.Recomendacion.ToList());
        }
        [HttpGet]
        public ActionResult Delete(int RecomendacionID)
        {
            new RecomendationRepository().Delete(RecomendacionID);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            var grupos = new GroupRepository().GetAll();
            var tiemposcomida = MyGymContext.DB.TiempoDeComida.AsEnumerable();
            return View(new RecomendationModel() { TiemposComida = tiemposcomida, Grupos = grupos});
        }
        [HttpPost]
        public ActionResult Create(Recomendacion recomendation, IEnumerable<int> tiemposdecomida, string urlimage)
        {
            return Json(new { rec = recomendation, tc = tiemposdecomida, urlimage = urlimage }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetFoods(int groupid)
        {
            var result = new FoodRepository().GetByGroupID(groupid).Select(item => new { id = item.AlimentoID, name = item.Nombre}); ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
    public class RecomendationModel
    {
        public IEnumerable<TiempoDeComida> TiemposComida { get; set; }
        public IEnumerable<Grupo> Grupos { get; set; }
    }
}
