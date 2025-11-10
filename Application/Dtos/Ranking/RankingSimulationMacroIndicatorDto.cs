using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking
{
    public class RankingSimulationMacroIndicatorDto
    {
        public required int Id { get; set; }

        public required decimal WeightFactor { get; set; }

        public RankingMacroIndicatorDto? MacroIndicator { get; set; }
    }
}
