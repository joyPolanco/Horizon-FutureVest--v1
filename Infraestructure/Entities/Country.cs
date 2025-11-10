using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class Country:BasicEntity<int>
    {
       
        public required string IsoCode { get; set; }
        public ICollection<EconomicIndicator> EconomicIndicators { get; set; } = new List<EconomicIndicator>();
    }
}
