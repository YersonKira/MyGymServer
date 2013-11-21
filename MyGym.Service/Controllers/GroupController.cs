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
    public class GroupController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var result = new GroupRepository().GetAll();
            return View(result);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Grupo groupdata)
        {
            int result = new GroupRepository().Add(groupdata);
            if (result == 0)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int groupid)
        {
            new GroupRepository().Delete(groupid);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int groupid)
        {
            var result = new GroupRepository().Get(groupid);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(int groupid, Grupo groupdata)
        {
            new GroupRepository().Update(groupid, groupdata);
            return RedirectToAction("Index");
        }
    }
}
