using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using credit_calculator.Models;
using Newtonsoft.Json;
using Calabonga.Xml.Exports;

namespace credit_calculator.Controllers 
{

    public class HomeController : Controller 
    {
        public ActionResult CalculateCredit() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result(Credit credit, string rate) 
        {

            if ((rate == "в день") && (credit.CreditPeriod % credit.PaymentStep != 0))
            {
                ModelState.AddModelError("Credit", "Срок кредита должен быть кратен шагу платежа");
            }
            if ((rate == "в день") && (credit.CreditPeriod % credit.PaymentStep != 0) && (credit.PaymentStep == 1))
            {
                ModelState.AddModelError("Credit", "Введите шаг платежа");
            }

            if (ModelState.IsValid) 
            {
                Payment[] payments;

                if (rate == "в день") 
                {
                    payments = PaymentScheduleForDayRate(credit);
                }
                else 
                {
                    payments = PaymentScheduleForYearRate(credit);
                }

                return View(payments);
            }
            return View("CalculateCredit");
        }

        private Payment[] PaymentScheduleForYearRate(Credit data) 
        {
            double thisPeriod = data.CreditPeriod;
            double ratePerMonth = data.CreditRate / 100 / 12;
            double payPerMonth = data.CreditAmount * (ratePerMonth / (1 - Math.Pow(1 + ratePerMonth, -data.CreditPeriod)));
            double totalCreditAmount = payPerMonth * data.CreditPeriod;

            Payment[] pay = data.Initialization(thisPeriod, payPerMonth, totalCreditAmount, true);

            ViewData["payPer"] = payPerMonth.ToString("0.00");
            ViewData["totalOverpay"] = pay[0].TotalOverPay.ToString("0.00");
            return pay;
        }

        private Payment[] PaymentScheduleForDayRate(Credit data) 
        {
            double thisPeriod = data.CreditPeriod / data.PaymentStep;
            double ratePerStep = data.CreditRate / 100 * data.PaymentStep;
            double payPerStep = data.CreditAmount * ((ratePerStep * Math.Pow((1 + ratePerStep),(data.CreditPeriod/data.PaymentStep)))/ (Math.Pow((1 + ratePerStep), (data.CreditPeriod / data.PaymentStep)) - 1));
            double totalCreditAmount = payPerStep * data.CreditPeriod / data.PaymentStep;

            Payment[] pay = data.Initialization(thisPeriod, payPerStep, totalCreditAmount, false);

            ViewData["payPer"] = payPerStep.ToString("0.00");
            ViewData["totalOverpay"] = pay[0].TotalOverPay.ToString("0.00");
            return pay;
        }
    }
}