using CurrencyConv.Controllers;
using CurrencyConv.Helpers;
using CurrencyConv.Helpers.Interfaces;
using CurrencyConv.Models.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Http.Results;

namespace CurrencyConv.Tests
{
    [TestClass]
    public class TestConversionController
    {
        private ICurrencyHelper _currencyHelper;

        private IXMLDataHelper _XMLDataHelper;

        private ISettingsHelper _settingsHelper;

        private ConversionController cc;

        [TestInitialize]
        public void Initialize()
        {
            _settingsHelper = new SettingsHelper();
            _XMLDataHelper = new XMLDataHelper(_settingsHelper);
            _currencyHelper = new CurrencyHelper(_XMLDataHelper, _settingsHelper);
            cc = new ConversionController(_currencyHelper);
        }

        [TestMethod]

        public void Test_ConvertAmount()
        {
         
            ConvertDto convertDto = new ConvertDto() { FromCurrencyName = "EUR", ToCurrencyName = "USD", Amount = 50 };
            var result = cc.ConvertAmount(convertDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            var fromRate = _currencyHelper.GetLatestCurrencyExchangeRate(convertDto.FromCurrencyName);
            fromRate.Wait();
            var toRate = _currencyHelper.GetLatestCurrencyExchangeRate(convertDto.ToCurrencyName);
            toRate.Wait();
            var amount = System.Convert.ToDecimal(convertDto.Amount, new CultureInfo("en-US"));
            var expectedAmount = amount / fromRate.Result * toRate.Result;

            Assert.AreEqual(expectedAmount, value.Content);

        }

        [TestMethod]
        public void Test_ConvertAmount_IncorrectCurrencyName()
        {
            ConvertDto convertDto = new ConvertDto() { FromCurrencyName = "ANY", ToCurrencyName = "USD", Amount = 10 };
            var result = cc.ConvertAmount(convertDto);
            result.Wait();

            var value = (JsonResult<Decimal>)result.Result;
            Assert.AreEqual(Convert.ToDecimal(0), value.Content);

        }

        [TestMethod]
        public void Test_CurrencyHistoricalRateByDate()
        {
            HistoricalRateByDateReqDto hisRateDto = new HistoricalRateByDateReqDto() { CurrencyName = "USD", Date = Convert.ToDateTime("2021-05-18") };
            var result = cc.CurrencyHistoricalRateByDate(hisRateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(1.2222), value.Content);

        }
        [TestMethod]
        public void Test_CurrencyHistoricalRateByDate_DateOutofRange()
        {
            HistoricalRateByDateReqDto hisRateDto = new HistoricalRateByDateReqDto() { CurrencyName = "USD", Date = Convert.ToDateTime("2021-05-29") };
            var result = cc.CurrencyHistoricalRateByDate(hisRateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(0), value.Content);

        }
        [TestMethod]
        public void Test_CurrencyHistoricalRateByDate_DateOutofRange_EUR()
        {
            HistoricalRateByDateReqDto hisRateDto = new HistoricalRateByDateReqDto() { CurrencyName = "EUR", Date = Convert.ToDateTime("2021-05-29") };
            var result = cc.CurrencyHistoricalRateByDate(hisRateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(1), value.Content);

        }
        [TestMethod]
        public void Test_CurrencyHistoricalRateByDate_IncorrectCurrencyName()
        {
            HistoricalRateByDateReqDto hisRateDto = new HistoricalRateByDateReqDto() { CurrencyName = "ABC", Date = Convert.ToDateTime("2021-05-29") };
            var result = cc.CurrencyHistoricalRateByDate(hisRateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(0), value.Content);

        }

        [TestMethod]
        public void Test_ConvertCurrencyByHistoricalDate()
        {
            ConvertCurrencyByHistoricalDateReqDto ccByDateDto = new ConvertCurrencyByHistoricalDateReqDto() { FromCurrencyName = "EUR", ToCurrencyName = "USD", Amount = 10, Date = Convert.ToDateTime("2021-05-18") };
            var result = cc.ConvertCurrencyByHistoricalDate(ccByDateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(12.222), value.Content);

        }
        [TestMethod]
        public void Test_ConvertCurrencyByHistoricalDate_DateOutofRange()
        {
            ConvertCurrencyByHistoricalDateReqDto ccByDateDto = new ConvertCurrencyByHistoricalDateReqDto() { FromCurrencyName = "EUR", ToCurrencyName = "USD", Amount = 10, Date = Convert.ToDateTime("2021-05-30") };
            var result = cc.ConvertCurrencyByHistoricalDate(ccByDateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(0), value.Content);

        }
        [TestMethod]
        public void Test_ConvertCurrencyByHistoricalDate_DateOutofRange_EUR()
        {
            ConvertCurrencyByHistoricalDateReqDto ccByDateDto = new ConvertCurrencyByHistoricalDateReqDto() { FromCurrencyName = "EUR", ToCurrencyName = "EUR", Amount = 10, Date = Convert.ToDateTime("2021-05-30") };
            var result = cc.ConvertCurrencyByHistoricalDate(ccByDateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(10), value.Content);

        }
        [TestMethod]
        public void Test_ConvertCurrencyByHistoricalDate_IncorrectCurrencyName()
        {
            ConvertCurrencyByHistoricalDateReqDto ccByDateDto = new ConvertCurrencyByHistoricalDateReqDto() { FromCurrencyName = "ANY", ToCurrencyName = "EUR", Amount = 10, Date = Convert.ToDateTime("2021-05-30") };
            var result = cc.ConvertCurrencyByHistoricalDate(ccByDateDto);
            result.Wait();
            var value = (JsonResult<Decimal>)result.Result;

            Assert.IsNotNull(value);
            Assert.AreEqual(Convert.ToDecimal(0), value.Content);

        }

        [TestMethod]
        public void Test_CurrencyRateTimeLine()
        {

            var result = cc.CurrencyRateTimeLine("USD");
            result.Wait();
            var values = result.Result;
            var latestRate = _currencyHelper.GetLatestCurrencyExchangeRate("USD");
            latestRate.Wait();
            Assert.IsNotNull(values);
            Assert.AreEqual(latestRate.Result, values.ToList().FirstOrDefault().Rate);
           

        }

        [TestMethod]
        public void Test_CurrencyRateTimeLine_IncorrectCurrencyName()
        {

            var result = cc.CurrencyRateTimeLine("ANY");
            result.Wait();
            var values = result.Result;

            Assert.AreEqual(0,values.First().Rate);

        }

        [TestMethod]
        public void Test_CurrencyRateTimeLine_EUR()
        {

            var result = cc.CurrencyRateTimeLine("EUR");
            result.Wait();
            var values = result.Result;

            Assert.AreEqual(1, values.First().Rate);

        }


        [TestMethod]
        public void Test_GetAllCurrencies()
        {

            var result = cc.GetAllCurrencies();
            result.Wait();
            var values = result.Result;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Contains("USD"));
        }

    }
}
