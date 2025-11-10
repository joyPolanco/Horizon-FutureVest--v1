using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Ranking
{
    public class RankingPageFilterViewModel
    {
        public required int CurrentYearSelection { get; set; }
        public required List<RankingCountryViewModel> Countries { get; set; }

        public List<int>? Years { get; set; }
    }
}
