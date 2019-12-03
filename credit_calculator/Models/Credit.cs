﻿using System;
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
        [Range(20000.00, 600000.00, ErrorMessage = "Допустимый диапозон: 20000 - 250000")]
        [NotValidSum(ErrorMessage = "Диапозон: 20000 - 250000 (до 2х знаков после ',')")]
        public double CreditAmount { get; set; }

        [DisplayName("Срок кредита (в месяцах):")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+", ErrorMessage = "Положительное целочисленное число")]
        [Range(1, 60, ErrorMessage = "Допустимый диапозон: 1 - 36")]
        [NotValidTerm(ErrorMessage = "Целочисленное число в диапозоне: 1 - 36")]
        public double CreditPeriod { get; set; }

        [DisplayName("Процентная ставка (в год):")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Положительное число (до 2х знаков после ',')")]
        [Range(10.00, 25.00, ErrorMessage = "Допустимый диапозон: 10 - 25")]
        [NotValidRate(ErrorMessage = "Положительное число (до 2х знаков после ',')")]
        public double RatePerYear { get; set; }
    }

    public class Payment
    {

        public int Id { get; set; }
        public double Date { get; set; }
        public double Body { get; set; }
        public double Percent { get; set; }
        public double MainBalance { get; set; }

    }
}