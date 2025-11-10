using Application.Dtos.EconomicIndicator;
using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking
{
    public class RankingCountryWithIndicatorsDto :BasicEntityDto<int>
    {
     
        public required string IsoCode { get; set; }
        public required List<RankingEconomicIndicatorDto> Indicators { get; set; } 
    }
}
