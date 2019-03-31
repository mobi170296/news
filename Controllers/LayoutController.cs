﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsApplication.Controllers
{
    public class LayoutController : Controller
    {
        // GET: Layout
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Header()
        {
            return PartialView();
        }
        public PartialViewResult Footer()
        {
            return PartialView();
        }
    }
}