using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.MacroeconomicIndicatorSimulation
{
    public class SimulationMacroindicatorDto
    {
        public required int Id { get; set; }

        public required decimal WeightFactor { get; set; }

        public  string? Name { get; set; } = string.Empty;

        public required int MacroindicatorId { get; set; }
    }
}
