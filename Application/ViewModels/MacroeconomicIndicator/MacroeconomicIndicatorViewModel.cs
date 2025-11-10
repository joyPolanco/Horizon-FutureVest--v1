using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.MacroeconomicIndicator
{
    public class MacroeconomicIndicatorViewModel:BasicViewModel<int>
    {
        public required decimal WeightFactor { get; set; }

        public required bool HigherIsBetter { get; set; }
    }
}
