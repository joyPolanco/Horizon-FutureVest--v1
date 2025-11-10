using Application.ViewModels.Ranking;
using Application.ViewModels.RankingSimulation.MacroindicatorSimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulation
{
    public class SimulatorPageFilterViewModel
    {
        public List<RankingCountryViewModel> CountryList { get; set; }
        public List<int> IndicatorsYear { get; set; }
        public List <SimulationMacroindicatorViewModel> Macroindicators { get; set; }
        public int SelectedYear { get; set; }

    }
}
