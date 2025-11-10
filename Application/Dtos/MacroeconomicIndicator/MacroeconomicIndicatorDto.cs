using Application.Dtos.EconomicIndicator;
using Persistence.Common;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.MacroeconomicIndicatorDto
{
    public class MacroeconomicIndicatorDto :BasicEntityDto<int>
    {
        public required decimal WeightFactor { get; set; }

        public required bool HigherIsBetter { get; set; }

    }
}
