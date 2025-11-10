using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EconomicIndicator
{
    public class EconomicIndicatorSearch
    {
        public List<EconomicIndicatorDto> indicatorDto { get; set; }
        public  int SelectedCountryId { get; set; }
        public int SelectedYear { get; set; }
    }
}
