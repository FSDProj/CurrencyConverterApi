using currency.Model;
using CurrencyConv.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace CurrencyConv.Helpers
{
    public class XMLDataHelper: IXMLDataHelper
    {

        private readonly ISettingsHelper _settingsHelper;

        public XMLDataHelper(ISettingsHelper settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }
        public async Task<HttpResponseMessage> FetchExchangeRateXML()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var path = ConfigurationManager.AppSettings["CurrencyDatasourceUrl"];
               
                response =  await client.GetAsync(path);
            }
            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception("error");
            }

            return response;
           
        }

        public async Task<List<CubeTime>> SerializeRawXMLwithModel(HttpResponseMessage response )
        {
            List<CubeTime> listCubeTime = new List<CubeTime>();
            XmlDocument xmldoc = new XmlDocument();

            var rawMsg = await response.Content.ReadAsStringAsync();

            xmldoc.LoadXml(rawMsg);
            var parentNode = xmldoc.GetElementsByTagName("gesmes:Envelope")[0];
            var cubeNodes = parentNode.LastChild.ChildNodes;
            foreach (XmlNode xn in cubeNodes)
            {
                var tempCubes = new CubeTime();
                string time = xn.Attributes["time"].Value;
                tempCubes.Date = Convert.ToDateTime(time);
                var innerCubeNodes = xn.ChildNodes;
                foreach (XmlNode cubeNode in innerCubeNodes)
                {
                    string currency = cubeNode.Attributes["currency"].Value;
                    Decimal rate = Convert.ToDecimal(cubeNode.Attributes["rate"].Value);
                    tempCubes.listCubeRate.Add(new CubeRate
                    {
                        CurrencyName = currency,
                        Rate = rate
                    }
                    );
                }
                listCubeTime.Add(tempCubes);
            }
            return listCubeTime;
        }
    }
}