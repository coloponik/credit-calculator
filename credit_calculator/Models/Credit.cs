using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using credit_calculator.Annotations;

namespace credit_calculator.Models
{
    public class Credit
    {
        [DisplayName("Сумма кредита:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Положительное число (до 2х знаков после ',')")]
        [Range(20000.00, 500000.00, ErrorMessage = "Допустимый диапозон: 20000 - 250000")]
        [NotValidSum(ErrorMessage = "Диапозон: 20000 - 250000 (до 2х знаков после ',')")]
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
        [Range(1, 60, ErrorMessage = "Допустимый диапозон: 1 - 36")]
        [NotValidTerm(ErrorMessage = "Целочисленное число в диапозоне: 1 - 36")]
        public double CreditPeriod { get; set; }

        [DisplayName("Шаг платежа (в днях):")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+", ErrorMessage = "Положительное целочисленное число")]
        [Range(1, 30, ErrorMessage = "Допустимый диапозон: 1 - 30")]
        [NotValidTerm(ErrorMessage = "Целочисленное число в диапозоне: 1 - 30")]
        public double PaymentStep { get; set; }
    }

    public class Payment
    {

        public int Id { get; set; }
        public string Date { get; set; }
        public double Body { get; set; }
        public double Percent { get; set; }
        public double MainBalance { get; set; }
    }
}