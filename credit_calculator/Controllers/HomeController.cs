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
                Payment[] payments = PaymentScheduleCalculation(credit);
                return View(payments);
            }
            return View("CalculateCredit");
        }

        public ActionResult ClearCredit()
        {
            return View("CalculateCredit");
        }

        private Payment[] PaymentScheduleCalculation(Credit data)
        {
            double ratePerMonth = data.RatePerYear / 100 / 12;
            double payPerMonth = data.CreditAmount * (ratePerMonth / (1 - Math.Pow(1 + ratePerMonth, -data.CreditPeriod)));
            double totalCreditAmount = payPerMonth * data.CreditPeriod;

            double creditAmountCopy = data.CreditAmount;
            double totalCreditAmountCopy = totalCreditAmount;
            double totalPlus = 0;
            Payment[] pay = new Payment[(int)data.CreditPeriod];
            for (int i = 0; i < data.CreditPeriod; ++i)
            {
                pay[i] = new Payment();
                double percent = creditAmountCopy * (data.RatePerYear / 100) / 12;
                creditAmountCopy -= payPerMonth - percent;
                //-------------------------------------------------------------------------------------
                pay[i].Id = i + 1;                     //№ платежа
                pay[i].Date = payPerMonth;             //дата платежа
                pay[i].Body = payPerMonth - percent;   //размер платежа по телу
                pay[i].Percent = percent;              //размер платежа по %
                pay[i].MainBalance = creditAmountCopy; //остаток основного долга
                totalCreditAmountCopy -= payPerMonth;
                totalPlus = pay[i].MainBalance;
                //-------------------------------------------------------------------------------------
            }
            double totalOverpay = totalCreditAmount - data.CreditAmount + totalPlus;
            ViewData["totalOverpay"] = totalOverpay;
            return pay;
        }
    }
}