﻿using MyGym.Common;
using MyGym.Data.Entities;
using MyGym.Service.Models;
using MyGym.Service.Models.APIHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGym.Service.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult Index(int userid)
        {
            var result = (new UserRepository()).Get(userid);
            var usuario = APIFunctions.GetData<UserInformation>(JsonConvert.SerializeObject(result));
            return View(APIFunctions.UserToUsuario(usuario));
        }
        [HttpPost]
        public ActionResult LogIn(string user, string password)
        {
            var result = (new UserRepository()).LogIn(user, password);
            var usuario = APIFunctions.GetData<UserInformation>(JsonConvert.SerializeObject(result));
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", new { userid = usuario.UserID });
        }
        [HttpGet]
        public ActionResult Register() {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserInformation userdata, string DateOfBirth)
        {
            userdata.DateOfBirth = DateTime.Parse(DateOfBirth);
            var result = (new UserRepository()).Add(userdata);
            var usuarioID = APIFunctions.GetData<int>(JsonConvert.SerializeObject(result));
            if (usuarioID == default(int))
            {
                return RedirectToAction("Register");
            }
            return RedirectToAction("Index", new { userid = usuarioID });  
        }
        [HttpGet]
        public ActionResult Edit(int userid)
        {
            var result = new UserRepository().Get(userid);
            var user = APIFunctions.GetData<UserInformation>(JsonConvert.SerializeObject(result));
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(int userid, UserInformation userdata, string DateOfBirth)
        {
            userdata.DateOfBirth = DateTime.Parse(DateOfBirth);
            var result = new UserRepository().Update(userid, userdata);
            var data = APIFunctions.GetData<int>(JsonConvert.SerializeObject(result));
            if (data == default(int))
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", new { userid = userid });
        }
    }
}
