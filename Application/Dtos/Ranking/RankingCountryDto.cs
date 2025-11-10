using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ranking
{
    public class RankingCountryDto
    {
        public required string CountryName { get; set; }
        public required string CountryIsoCode { get; set; }

        public decimal Score { get; set; }
        public decimal ReturnInvestmentRate { get; set; }

    }
}
