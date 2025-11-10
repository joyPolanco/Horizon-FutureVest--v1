using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EconomicIndicator
{
    public class EconomicIndicatorDto
    {
        public required int Id { get; set; }

        public required int CountryId { get; set; }
        public required string CountryName { get; set; } 
        public required string MacroeconomicIndicatorName { get; set; }

        public required int MacroeconomicIndicatorId { get; set; }
        public required decimal Value { get; set; }
        public required int Year { get; set; }
    }
}
