using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class MacroIndicatorBaseEntity 
    {
        public required int Id { get; set; }

        public required decimal WeightFactor { get; set; }

    }
}
