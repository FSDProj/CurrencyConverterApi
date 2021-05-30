using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConv.Helpers.Interfaces
{
    public interface ISettingsHelper
    {
        string XMLUrl { get; }
        string TimeInterval { get; }
    }
}
