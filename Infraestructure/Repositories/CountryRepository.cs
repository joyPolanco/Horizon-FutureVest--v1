using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Persistence.Contexts;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CountryRepository : GenericRepository<Country>
    {
        public CountryRepository(FutureVestContext context) : base(context)
        {
          
        }
        public async Task<Country?> SearchByIsoAndNameAsync(string iso, string name)
        {

           return await _context.Set<Country>()
                .Where(c=>
                   c.IsoCode.ToLower() == iso.ToLower() ||
                   c.Name.ToLower() ==name.ToLower())
                   .FirstOrDefaultAsync();
        }


    }
}
