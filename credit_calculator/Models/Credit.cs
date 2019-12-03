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
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Должно быть положительным числом (до 2х знаков после ',')")]
        [Range(20000.00, 250000.00, ErrorMessage = "Допустимый диапозон: 20000 - 250000")]
        [NotValidSum(ErrorMessage = "Допустимый диапозон: 20000 - 250000 (до 2х знаков после ',')")]
        public double CreditSum { get; set; }

        [DisplayName("Срок кредита (в месяцах):")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+", ErrorMessage = "Должно быть положительным целочисленным числом")]
        [Range(1, 36, ErrorMessage = "Допустимый диапозон: 1 - 36")]
        [NotValidTerm(ErrorMessage = "Должно быть целочисленным числом в диапозоне: 1 - 36")]
        public double CreditPeriod { get; set; }

        [DisplayName("Процентная ставка (в год):")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Должно быть положительным числом (до 2х знаков после ',')")]
        [Range(10.00, 25.00, ErrorMessage = "Допустимый диапозон: 10 - 25")]
        [NotValidRate(ErrorMessage = "Должно быть положительным числом (до 2х знаков после ',')")]
        public double CreditRateYear { get; set; }
    }
}