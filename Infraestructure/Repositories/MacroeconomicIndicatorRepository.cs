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
    public class MacroeconomicIndicatorRepository : GenericRepository<MacroeconomicIndicator>
    {
        public MacroeconomicIndicatorRepository(FutureVestContext context) : base(context)
        { }

             public async Task<MacroeconomicIndicator?> SearchByNameAsync( string name)
            {

                return await _context.Set<MacroeconomicIndicator>()
                        .Where(mci =>
                        mci.Name.ToLower() == name.ToLower())
                        .FirstOrDefaultAsync();
            }



    }
    }

