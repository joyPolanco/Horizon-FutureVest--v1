using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EstimatedConfiguration
{
    public class EstimatedRateConfigurationDto
    {
        public required decimal MinRate { get; set; }
        public required decimal MaxRate { get; set; }
    }
}
