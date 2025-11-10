using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Ranking
{
    public class RankingCountryViewModel
    {
        public required string CountryName { get; set; }
        public required string CountryIsoCode { get; set; }

        public required string Score { get; set; }
        public required string ReturnInvestmentRate { get; set; }

    }
}
