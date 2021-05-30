using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace currency.Model
{
   public class CubeTime
    {
        public CubeTime()
        {
            listCubeRate = new List<CubeRate>();
        }
        public DateTime Date { get; set; }
        public List<CubeRate> listCubeRate { get; set; }
    }
}
