using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.MacroeconomicIndicatorSimulation
{
    public class SaveSimulationMacroindicatorDto
    {
        public required int Id { get; set; }

        public required decimal WeightFactor { get; set; }

        public required int MacroindicatorId { get; set; }
    }
}
