using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class EconomicIndicator
    {
        public required int Id { get; set; }
        public required int CountryId { get; set; }
        public  Country? Country { get; set; }
        public required int MacroeconomicIndicatorId { get; set; }
        public  MacroeconomicIndicator ?MacroeconomicIndicator { get; set; }
        public required decimal Value { get; set; }
        public required int Year { get; set; }

    }
}
