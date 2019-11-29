using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace credit_calculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CalculateCredit()
        {
            return View();
        }

        public ActionResult ClearCredit()
        {
            return RedirectToAction("Index");
        }
    }
}