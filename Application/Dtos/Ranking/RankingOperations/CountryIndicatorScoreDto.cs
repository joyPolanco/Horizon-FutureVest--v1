using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking.RankingOperations
{
    public class CountryIndicatorScoreDto
    {
        public int IndicatorId { get; set; }
        public decimal Value { get; set; }
        public decimal SubScore { get; set; }


    }
}
