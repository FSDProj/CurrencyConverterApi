using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CurrencyConv.Helpers.Interfaces
{
    public class SettingsHelper: ISettingsHelper
    {
        public string XMLUrl => ConfigurationManager.AppSettings["CurrencyDatasourceUrl"];
        public string TimeInterval => ConfigurationManager.AppSettings["IntervalInMinutes"];
    }
}