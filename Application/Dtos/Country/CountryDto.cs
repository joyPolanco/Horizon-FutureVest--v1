using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Country
{
    public class CountryDto :BasicEntityDto<int>
    {
        public required string IsoCode { get; set; }

    }
}
