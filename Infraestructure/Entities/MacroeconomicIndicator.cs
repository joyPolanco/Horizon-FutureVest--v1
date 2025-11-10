using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class MacroeconomicIndicator : MacroIndicatorBaseEntity
    {

        public required bool HigherIsBetter { get; set; }

        public ICollection<EconomicIndicator>? EconomicIndicators { get; set; } = new List<EconomicIndicator>();
        public required string Name { get; set; }

        public SimulationMacroIndicator? SimulationMacroIndicator { get; set; }

    }
}
