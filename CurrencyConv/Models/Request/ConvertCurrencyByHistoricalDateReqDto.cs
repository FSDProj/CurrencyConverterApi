using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CurrencyConv.Models.Request
{
    public class ConvertCurrencyByHistoricalDateReqDto
    {
        [Required]
        public string FromCurrencyName { get; set; }
        [Required]
        public string ToCurrencyName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}