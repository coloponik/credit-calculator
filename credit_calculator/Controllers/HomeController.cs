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
        public ActionResult GetCsvFile(Payment[] Model)
        {
            ExcelResult export = Export(Model);
            return View("Result", export);
        }

        public ExcelResult Export(Payment[] dt)
        {
            string result = string.Empty;
            Workbook wb = new Workbook();

            wb.Properties.Author = "coloponik";
            wb.Properties.Created = DateTime.Today;
            wb.Properties.LastAutor = "coloponik";
            wb.Properties.Version = "1";

            // options sheets
            wb.ExcelWorkbook.ActiveSheet = 1;
            wb.ExcelWorkbook.DisplayInkNotes = false;
            wb.ExcelWorkbook.FirstVisibleSheet = 1;
            wb.ExcelWorkbook.ProtectStructure = false;
            wb.ExcelWorkbook.WindowHeight = 800;
            wb.ExcelWorkbook.WindowTopX = 0;
            wb.ExcelWorkbook.WindowTopY = 0;
            wb.ExcelWorkbook.WindowWidth = 600;

            // create style s1 for header
            Style s1 = new Style("s1");
            s1.Font.Bold = true;
            s1.Font.Italic = true;
            s1.Font.Color = "#FF0000";
            wb.AddStyle(s1);

            // create style s2 for header
            Style s2 = new Style("s2");
            s2.Font.Bold = true;
            s2.Font.Italic = true;
            s2.Font.Size = 12;
            s2.Borders.Add(new Border());
            s2.Font.Color = "#0000FF";
            wb.AddStyle(s2);

            Worksheet ws3 = new Worksheet("Пользователи");

            // Adding Headers
            ws3.AddCell(0, 0, "Наименование (Name)");
            ws3.AddCell(0, 1, "Возраст (Age)");
            ws3.AddCell(0, 2, "Пол (Gender)");
            ws3.AddCell(0, 3, "Вес (Weight)");
            ws3.AddCell(0, 4, "Страна (Country)");

            int totalRows = 0;

            // appending rows with data
            for (int i = 0; i < dt.Length; i++)
            {
                ws3.AddCell(i + 1, 0, dt[i].Id);
                ws3.AddCell(i + 1, 1, dt[i].Date);
                ws3.AddCell(i + 1, 2, dt[i].Body);
                ws3.AddCell(i + 1, 3, dt[i].Percent);
                ws3.AddCell(i + 1, 4, dt[i].MainBalance);
                totalRows++;
            }
            totalRows++;

            wb.AddWorksheet(ws3);

            // generate xml 
            string workstring = wb.ExportToXML();

            // Send to user file
            return new ExcelResult("payment_graphic.xls", workstring);
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