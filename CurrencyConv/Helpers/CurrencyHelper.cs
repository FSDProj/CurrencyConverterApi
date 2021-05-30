using currency.Model;
using CurrencyConv.Helpers.Interfaces;
using CurrencyConv.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CurrencyConv.Helpers
{
    public class CurrencyHelper : ICurrencyHelper
    {
        public List<CubeTime> currencyList { get; set; }
        private DateTime lastUpdate { get; set; }

        private readonly IXMLDataHelper _XMLDataHelper;

        private readonly ISettingsHelper _settingsHelper;
        public CurrencyHelper(IXMLDataHelper XMLDataHelper, ISettingsHelper settingsHelper)
        {
            _XMLDataHelper = XMLDataHelper;
            _settingsHelper = settingsHelper;
        }
        public async Task<List<string>> GetAvailableCurrienciesForConversion()
        {
            await UpdateData();
            return currencyList.FirstOrDefault().listCubeRate.Select(x => x.CurrencyName).ToList();
        }       

        public async Task<decimal> GetHistoricalCurrencyExchangeRate(string currencyName, DateTime date)
        {

            await UpdateData();
            bool IsValidCurrencyName = await CheckValidCurrency(currencyName);
            bool IsValidDate = CheckValidDate(date);
            if (IsValidCurrencyName )
            {
                if (currencyName.ToUpperInvariant() == "EUR")
                {
                    return 1;
                }
                else if (IsValidDate)
                {
                    return currencyList.Where(x => x.Date.Date.ToShortDateString().Equals(date.ToShortDateString())).FirstOrDefault().
                        listCubeRate.Where(y => y.CurrencyName == currencyName.ToUpperInvariant()).FirstOrDefault().Rate;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        public async Task<decimal> GetLatestCurrencyExchangeRate(string currencyName)
        {
            await UpdateData();
            bool IsValidCurrencyName = await CheckValidCurrency(currencyName);
            if (IsValidCurrencyName)
            {
                if (currencyName.ToUpperInvariant() == "EUR")
                {
                    return 1;
                }
                else
                {
                    return currencyList.FirstOrDefault().listCubeRate.Where(x => x.CurrencyName == currencyName.ToUpperInvariant()).FirstOrDefault().Rate;
                }
            }
            else
                return 0;
        }

        public async Task<List<CurrencyRateTimeLapse>> GetCurrencyTimeLine(string currencyName)
        {
            await UpdateData();
            bool IsValidCurrencyName = await CheckValidCurrency( currencyName);
            List<CurrencyRateTimeLapse> lapses = new List<CurrencyRateTimeLapse>();
            if (IsValidCurrencyName && currencyName.ToUpperInvariant()!="EUR")
            {
                foreach (var cr in currencyList)
                {
                    lapses.Add(new CurrencyRateTimeLapse
                    {
                        Timestamp = cr.Date,
                        Rate = cr.listCubeRate.Where(x => x.CurrencyName == currencyName.ToUpperInvariant()).FirstOrDefault().Rate
                    });
                }
            }
            else
            {
                lapses.Add(new CurrencyRateTimeLapse { Timestamp = DateTime.Now, Rate = currencyName.ToUpperInvariant() =="EUR"?1:0 });
            }
            return lapses;
        }

        private async Task<bool> CheckValidCurrency(string currencyName)
        {
            List<string> availableCurrencyList = await GetAvailableCurrienciesForConversion();
            if (availableCurrencyList.Contains(currencyName.ToUpperInvariant()) || currencyName.ToUpperInvariant() =="EUR")
                return true;
            else
                return false;
        }

        private bool CheckValidDate(DateTime reqDate)
        {           
            if (currencyList.Where(x => x.Date.Date.ToShortDateString().Contains(reqDate.ToShortDateString())).Count() > 0)
                return true;
            else
                return false;
        }
        private async Task UpdateData()
        {
            var TimeDurationInMinutes = Convert.ToInt32(_settingsHelper.TimeInterval);
            var isCacheExpired = DateTime.Now.Subtract(lastUpdate).Minutes > TimeDurationInMinutes;
            if (currencyList != null && isCacheExpired == false)
            {
                return;
            }

            var EUResponseMessage = await _XMLDataHelper.FetchExchangeRateXML();

            currencyList = await _XMLDataHelper.SerializeRawXMLwithModel(EUResponseMessage);
            lastUpdate = DateTime.Now;
        }
    }
}