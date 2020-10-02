﻿using Appointment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace Appointment.Controllers
{
    public class RolesController : AdminController
    {
        private ApplicationDbContext context = new ApplicationDbContext();
     
        public ActionResult RolesIndex()
        {
            List<RolesViewModel> rolesViewModels = new List<RolesViewModel>();
            foreach (var item in context.Roles.Where(m=>m.Name!="Admin").ToList())
            {
                rolesViewModels.Add(new RolesViewModel()
                {
                    RoleId=item.Id,
                    RoleName=item.Name
                });
            }
            return View(rolesViewModels);
        }
   
        public ActionResult AddRole()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddRole(string role)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!string.IsNullOrEmpty(role))
            {
                roleManager.Create(new IdentityRole(role));
                return RedirectToAction("RolesIndex");
            }
            return View();
        }
      
        public ActionResult EditRole(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole role = roleManager.FindById(id);
            RolesViewModel roleFound = new RolesViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            return View(roleFound);
        }
 
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditRole(RolesViewModel role)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (role != null)
            {
                var updateRole = roleManager.FindById(role.RoleId);
                updateRole.Name = role.RoleName;
                roleManager.Update(updateRole);
                return RedirectToAction("RolesIndex");
            }
            return View();
        }
        [HttpGet]
        public JsonResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    roleManager.Delete(roleManager.FindById(id));
                }
            }
            catch
            {

                
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}