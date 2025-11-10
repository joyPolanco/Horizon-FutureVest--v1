using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.EconomicIndicator
{
    public class DeleteEconomicIndicatorViewModel
    {
        public required int Id { get; set; }
        public required string CountryName { get; set; }

        public required decimal Value { get; set; }
        public required int Year { get; set; }

        public required string MacroIndicatorName { get; set; }
    }
}
