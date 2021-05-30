using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CurrencyConv.Models.Request
{
    public class HistoricalRateByDateReqDto
    {
        [Required]
        public string CurrencyName { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}