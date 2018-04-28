﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.PropertyManagement;
using JazMax.Web.ViewModel.PropertyManagement;

namespace JazMax.Web.Areas.Property.Controllers
{
    public class PropertyTypeController : Controller
    {
        private static PropertyTypeService o = new PropertyTypeService();

        public ActionResult Index()
        {
            return View(o.GetAll(true));
        }

 
        public JsonResult CreateType(string txtTypeName)
        {
            try
            {
                PropertyTypeView model = new PropertyTypeView()
                {
                    TypeName = txtTypeName
                };
                o.Create(model);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(o.GetById((int)Id));
        }

       
    }
}