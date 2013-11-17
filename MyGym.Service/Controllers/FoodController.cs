using MyGym.Data.Entities;
using MyGym.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGym.Service.Controllers
{
    public class FoodController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var result = new FoodRepository().GetAll();
            return View(result);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var groups = new GroupRepository().GetAll();
            return View(groups);
        }
        [HttpPost]
        public JsonResult Create(string fooddata)
        {
            Alimento food = JsonConvert.DeserializeObject<Alimento>(fooddata);
            string redirect = new UrlHelper(Request.RequestContext).Action("Index");
            var result = new FoodRepository().Add(food);
            if (result == 0)
            {
                redirect = new UrlHelper(Request.RequestContext).Action("Create");
            }
            return Json(new { Url = redirect });
        }
        [HttpGet]
        public ActionResult Edit(int foodid)
        {
            ViewBag.Grupos = new GroupRepository().GetAll();
            var result = new FoodRepository().Get(foodid);
            return View(result);
        }
        [HttpPost]
        public JsonResult Update(int foodid, string fooddata)
        { 
            Alimento food = JsonConvert.DeserializeObject<Alimento>(fooddata);
            new FoodRepository().Update(foodid, food);
            string redirect = new UrlHelper(Request.RequestContext).Action("Index");
            return Json(new { Url = redirect });
        }
        [HttpGet]
        public ActionResult Delete(int foodid)
        {
            new FoodRepository().Delete(foodid);
            return RedirectToAction("Index");
        }

    }
}
