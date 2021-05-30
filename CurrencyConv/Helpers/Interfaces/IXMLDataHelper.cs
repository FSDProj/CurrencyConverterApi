using currency.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConv.Helpers.Interfaces
{
    public interface IXMLDataHelper
    {
        Task<HttpResponseMessage> FetchExchangeRateXML();

        Task<List<CubeTime>> SerializeRawXMLwithModel(HttpResponseMessage response);
    }
}
