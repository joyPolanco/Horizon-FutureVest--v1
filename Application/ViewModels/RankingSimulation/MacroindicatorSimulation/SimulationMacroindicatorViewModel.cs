using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulation.MacroindicatorSimulation
{
    public class SimulationMacroindicatorViewModel
    {
        public required int Id { get; set; }

        public required decimal WeightFactor { get; set; }

        public required string  Name { get; set; }
    }
}
