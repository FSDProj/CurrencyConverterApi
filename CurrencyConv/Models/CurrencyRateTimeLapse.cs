using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyConv.Models
{
    public class CurrencyRateTimeLapse
    {
        public DateTime Timestamp { get; set; }
        public decimal Rate { get; set; }
    }
}