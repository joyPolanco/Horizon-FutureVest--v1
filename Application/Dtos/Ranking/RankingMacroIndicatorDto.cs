using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking
{
    public class RankingMacroIndicatorDto : BasicEntityDto<int>
    {

        public decimal WeightFactor { get; set; }
        public bool HigherIsBetter
        {
            get; set;
        }

        public RankingSimulationMacroIndicatorDto ?SimulationMacroindicator {get;set ;}
    }
}
