using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking.RankingOperations
{
    public class RankingMacroIndicatorExtremeDto
    {
        public int MacroIndicadorId { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal Weight { get; set; }
        public bool HighIsBetter { get; set; }
    }
}
