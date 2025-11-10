using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Country
{
    public class CountryViewModel :BasicViewModel<int>
    {
        public required string IsoCode { get; set; }

    }
}
