using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class EstimatedRate
    {
        public required int Id { get; set; }
        public required decimal MinRate {  get; set; }
        public required decimal MaxRate { get; set; }
    }
}
