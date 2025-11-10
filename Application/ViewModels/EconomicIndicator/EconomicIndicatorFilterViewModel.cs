using Application.Dtos.Country;
using Application.Dtos.EconomicIndicator;
using Application.ViewModels.Country;
using Application.ViewModels.EconomicIndicator;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.EconomicIndicator
{
    public class EconomicIndicatorFilterViewModel
    {
       public required List<CountryViewModel> Countries { get; set; }
       public required List<EconomicIndicatorViewModel> EconomicIndicators{ get; set; }
        public required List<int> Years { get;set; }
      public  int ?YearCurrentSelection { get; set; }
      public  int ?CountryIdCurrentSelection { get; set; }

    }
}
