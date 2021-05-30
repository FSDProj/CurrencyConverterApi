using CurrencyConv.Helpers;
using CurrencyConv.Helpers.Interfaces;
using CurrencyConv.Models;
using CurrencyConv.Models.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CurrencyConv.Controllers
{
    public class ConversionController : ApiController
    {
        private ICurrencyHelper _currencyHelper;

        public ConversionController(ICurrencyHelper currencyHelper)
        {
            _currencyHelper = currencyHelper;
        }

        [HttpPost]
        [Route("api/CurrencyConverter/ConvertAmount")]
        public async Task<IHttpActionResult> ConvertAmount(ConvertDto convertDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fromRate = await _currencyHelper.GetLatestCurrencyExchangeRate(convertDto.FromCurrencyName);
                    var toRate = await _currencyHelper.GetLatestCurrencyExchangeRate(convertDto.ToCurrencyName);
                    var amount = System.Convert.ToDecimal(convertDto.Amount, new CultureInfo("en-US"));
                    if (fromRate != 0)
                    {
                        var result = amount / fromRate * toRate;
                        return await Task.FromResult(Json(result));
                    }
                    else
                        return await Task.FromResult(Json(Convert.ToDecimal(0)));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(e.Message) };
                throw new HttpResponseException(response);
            }

        }

        [HttpPost]
        [Route("api/CurrencyConverter/FetchCurrencyHistoricalRateByDate")]
        public async Task<IHttpActionResult> CurrencyHistoricalRateByDate(HistoricalRateByDateReqDto hisRateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _currencyHelper.GetHistoricalCurrencyExchangeRate(hisRateDto.CurrencyName, hisRateDto.Date);

                    return await Task.FromResult(Json(result));
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(e.Message) };
                throw new HttpResponseException(response);
            }
        }

        [HttpPost]
        [Route("api/CurrencyConverter/ConvertAmountByHistoricalDate")]
        public async Task<IHttpActionResult> ConvertCurrencyByHistoricalDate(ConvertCurrencyByHistoricalDateReqDto ccByDateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fromRate = await _currencyHelper.GetHistoricalCurrencyExchangeRate(ccByDateDto.FromCurrencyName, ccByDateDto.Date);
                    var toRate = await _currencyHelper.GetHistoricalCurrencyExchangeRate(ccByDateDto.ToCurrencyName, ccByDateDto.Date);
                    var amount = System.Convert.ToDecimal(ccByDateDto.Amount, new CultureInfo("en-US"));

                    if (fromRate != 0)
                    {
                        var result = amount / fromRate * toRate;
                        return await Task.FromResult(Json(result));
                    }
                    else
                        return await Task.FromResult(Json((Convert.ToDecimal(0))));
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(e.Message) };
                throw new HttpResponseException(response);
            }
        }

        [HttpGet]
        [Route("api/CurrencyConverter/CurrencyRateTimeLine")]
        public async Task<List<CurrencyRateTimeLapse>> CurrencyRateTimeLine(string currencyName)
        {
            try
            {
                var result = await _currencyHelper.GetCurrencyTimeLine(currencyName);
                return result;
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(e.Message) };
                throw new HttpResponseException(response);
            }
        }

        [HttpGet]
        [Route("api/CurrencyConverter/GetAllCurrencies")]
        public async Task<List<string>> GetAllCurrencies()
        {
            try
            {
                var result = await _currencyHelper.GetAvailableCurrienciesForConversion();
                return result;
            }
            catch (Exception e)
            {

                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(e.Message) };
                throw new HttpResponseException(response);
            }
        }
    }
}
