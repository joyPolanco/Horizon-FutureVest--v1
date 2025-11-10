using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Country
{
    public class DeleteCountryViewModel
    {
        public required int Id { get; set; }
        public string? Name { get; set; }

        public required string IsoCode { get; set; }

    }
}
