using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class BasicEntityDto<T>
    {
        public required T Id { get; set; }
        public required string Name { get; set; }
    }
}
