using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using credit_calculator.Models;

namespace credit_calculator.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult CalculateCredit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result(Credit credit)
        {
            if (ModelState.IsValid)
            {
                return View(credit);
            }
            return View("CalculateCredit");
        }

        public ActionResult ClearCredit()
        {
            return View("CalculateCredit");
        }
    }
}