using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using credit_calculator.Annotations;
using Calabonga.Xml.Exports;
using System.Text;

namespace credit_calculator.Models
{
    public class Credit
    {
        [DisplayName("Сумма кредита:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Положительное число (до 2х знаков после ',')")]
        [Range(20000.00, 500000.00, ErrorMessage = "Допустимый диапозон: 20000 - 500000")]
        [NotValidSum(ErrorMessage = "Диапозон: 20000 - 500000 (до 2х знаков после ',')")]
        public double CreditAmount { get; set; }

        [DisplayName("Процентная ставка:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Положительное число (до 2х знаков после ',')")]
        [Range(0.50, 25.00, ErrorMessage = "Допустимый диапозон: 0.5 - 25")]
        [NotValidRate(ErrorMessage = "Положительное число (до 2х знаков после ',')")]
        public double CreditRate { get; set; }

        [DisplayName("Срок кредита:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+", ErrorMessage = "Положительное целочисленное число")]
        [Range(1, 60, ErrorMessage = "Допустимый диапозон: 1 - 60")]
        [NotValidTerm(ErrorMessage = "Целочисленное число в диапозоне: 1 - 60")]
        public double CreditPeriod { get; set; }

        [DisplayName("Шаг платежа (в днях):")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+", ErrorMessage = "Положительное целочисленное число")]
        [Range(1, 30, ErrorMessage = "Допустимый диапозон: 1 - 30")]
        [NotValidTerm(ErrorMessage = "Целочисленное число в диапозоне: 1 - 30")]
        public double PaymentStep { get; set; }

        public Payment[] Initialization(double period, double payPerPeriod, double total, bool type)
        {
            Payment[] rezPay = new Payment[(int)period];

            double creditAmountCopy = CreditAmount;
            double totalCreditAmountCopy = total;
            double totalPlus = 0;
            DateTime today = DateTime.Today;

            for (int i = 0; i < period; ++i) 
            {
                rezPay[i] = new Payment();
                double percent = creditAmountCopy * (CreditRate / 100);
                if (type) 
                {
                    percent = percent / 12;
                }
                if (!type) 
                {
                    percent = percent * PaymentStep;
                }
                creditAmountCopy -= payPerPeriod - percent;
                //-------------------------------------------------------------------------------------
                rezPay[i].Id = i + 1;                                                               //№ платежа
                if (type) 
                {
                    rezPay[i].Date = today.AddMonths(i).ToString("dd/MM/yyyy");                     //дата платежа
                }
                if (!type) 
                {
                    rezPay[i].Date = today.AddDays(PaymentStep * i).ToString("dd/MM/yyyy");         //дата платежа
                }
                rezPay[i].Body = Math.Round(payPerPeriod - percent, 2);                             //размер платежа по телу
                rezPay[i].Percent = Math.Round(percent, 2);                                         //размер платежа по %
                rezPay[i].MainBalance = Math.Round(creditAmountCopy, 2);                            //остаток основного долга
                totalCreditAmountCopy -= Math.Round(payPerPeriod, 2);
                totalPlus = rezPay[i].MainBalance;
                //-------------------------------------------------------------------------------------
            }
            rezPay[0].TotalOverPay = Math.Round(total - CreditAmount + totalPlus, 2);
            return rezPay;
        }
    }

    public class Payment
    {

        public int Id { get; set; }
        public string Date { get; set; }
        public double Body { get; set; }
        public double Percent { get; set; }
        public double MainBalance { get; set; }
        public double TotalOverPay { get; set; }

    }

    public class ExcelResult : ActionResult
    {
        public ExcelResult(string fileName, string report)
        {
            this.Filename = fileName;
            this.Report = report;
        }
        public string Report { get; private set; }
        public string Filename { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
                                 string.Format("attachment; filename={0}", Filename));
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write(Report);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}