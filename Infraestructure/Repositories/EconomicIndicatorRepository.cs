using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class EconomicIndicatorRepository : GenericRepository<EconomicIndicator>
    {  
        public EconomicIndicatorRepository(FutureVestContext context) : base(context)
        {

        }


        public async Task<EconomicIndicator?> GetByIdWithInclude(int id)
        {
            return await  _context.Set<EconomicIndicator>()
                .Include(x=>x.Country)
                .Include(x=>x.MacroeconomicIndicator)
                .FirstOrDefaultAsync(x => x.Id == id);


        }
    }
}
