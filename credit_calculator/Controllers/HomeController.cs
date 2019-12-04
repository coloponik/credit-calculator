using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
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
                Payment[] payments = PaymentScheduleForYearRate(credit);
                return View(payments);
            }
            return View("CalculateCredit");
        }

        private Payment[] PaymentScheduleForYearRate(Credit data)
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
                DateTime today = DateTime.Today;
                //-------------------------------------------------------------------------------------
                pay[i].Id = i + 1;                                               //№ платежа
                pay[i].Date = today.AddMonths(i).ToString("dd/MM/yyyy");         //дата платежа
                pay[i].Body = Math.Round(payPerMonth - percent, 2);              //размер платежа по телу
                pay[i].Percent = Math.Round(percent, 2);                         //размер платежа по %
                pay[i].MainBalance = Math.Round(creditAmountCopy, 2);            //остаток основного долга
                totalCreditAmountCopy -= Math.Round(payPerMonth, 2);
                totalPlus = pay[i].MainBalance;
                //-------------------------------------------------------------------------------------
            }
            double totalOverpay = Math.Round(totalCreditAmount - data.CreditAmount + totalPlus, 2);
            ViewData["totalOverpay"] = totalOverpay;
            return pay;
        }
    }
}