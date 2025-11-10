using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking.RankingOperations
{
    public class CountryWithIndicatorsScoreDto
    {
        public int CountryId { get; set; }
        public required string IsoCode { get; set; }
        public required string CountryName { get; set; }

        public required List<CountryIndicatorScoreDto> Indicators { get; set; }
    }
}
