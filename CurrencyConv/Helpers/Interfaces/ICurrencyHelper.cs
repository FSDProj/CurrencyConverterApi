using currency.Model;
using CurrencyConv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConv.Helpers.Interfaces
{
    public interface ICurrencyHelper
    {
        List<CubeTime> currencyList { get; set; }
        Task<List<string>> GetAvailableCurrienciesForConversion();

        Task<decimal> GetLatestCurrencyExchangeRate(string currencyName);

        Task<decimal> GetHistoricalCurrencyExchangeRate(string currencyName, DateTime date);

        Task<List<CurrencyRateTimeLapse>> GetCurrencyTimeLine(string currencyName);
    }
}
