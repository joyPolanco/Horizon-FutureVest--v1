using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking
{
    public class RankingEconomicIndicatorDto
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public required  RankingMacroIndicatorDto MacroIndicator { get; set; } 
    }
}
