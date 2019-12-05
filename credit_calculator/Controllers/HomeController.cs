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
                Payment[] payments;
                if (ViewData["isChecked"] == (object)true) 
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
            double ratePerMonth = data.CreditRate / 100 / 12;
            double payPerMonth = data.CreditAmount * (ratePerMonth / (1 - Math.Pow(1 + ratePerMonth, -data.CreditPeriod)));
            double totalCreditAmount = payPerMonth * data.CreditPeriod;

            double creditAmountCopy = data.CreditAmount;
            double totalCreditAmountCopy = totalCreditAmount;
            double totalPlus = 0;
            Payment[] pay = new Payment[(int)data.CreditPeriod];
            DateTime today = DateTime.Today;
            for (int i = 0; i < data.CreditPeriod; ++i)
            {
                pay[i] = new Payment();
                double percent = creditAmountCopy * (data.CreditRate / 100) / 12;
                creditAmountCopy -= payPerMonth - percent;
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
            ViewData["payPer"] = payPerMonth.ToString("0.00");
            ViewData["totalOverpay"] = totalOverpay.ToString("0.00");
            return pay;
        }

        private Payment[] PaymentScheduleForDayRate(Credit data)
        {
            double ratePerStep = data.CreditRate / 100 * data.PaymentStep;
            double payPerStep = data.CreditAmount * ((ratePerStep * Math.Pow((1 + ratePerStep),(data.CreditPeriod/data.PaymentStep)))/ (Math.Pow((1 + ratePerStep), (data.CreditPeriod / data.PaymentStep)) - 1));
            double totalCreditAmount = payPerStep * data.CreditPeriod / data.PaymentStep;

            double creditAmountCopy = data.CreditAmount;
            double totalCreditAmountCopy = totalCreditAmount;
            double totalPlus = 0;
            Payment[] pay = new Payment[(int)(data.CreditPeriod / data.PaymentStep)];
            DateTime today = DateTime.Today;
            for (int i = 0; i < (data.CreditPeriod / data.PaymentStep); ++i)
            {
                pay[i] = new Payment();
                double percent = creditAmountCopy * (data.CreditRate / 100) * data.PaymentStep;
                creditAmountCopy -= payPerStep - percent;
                //-------------------------------------------------------------------------------------
                pay[i].Id = i + 1;                                                              //№ платежа
                pay[i].Date = today.AddDays(data.PaymentStep*i).ToString("dd/MM/yyyy");         //дата платежа
                pay[i].Body = Math.Round(payPerStep - percent, 2);                              //размер платежа по телу
                pay[i].Percent = Math.Round(percent, 2);                                        //размер платежа по %
                pay[i].MainBalance = Math.Round(creditAmountCopy, 2);                           //остаток основного долга
                totalCreditAmountCopy -= Math.Round(payPerStep, 2);
                totalPlus = pay[i].MainBalance;
                //-------------------------------------------------------------------------------------
            }
            double totalOverpay = Math.Round(totalCreditAmount - data.CreditAmount + totalPlus, 2);
            ViewData["payPer"] = payPerStep.ToString("0.00");
            ViewData["totalOverpay"] = totalOverpay.ToString("0.00");
            return pay;
        }
    }
}