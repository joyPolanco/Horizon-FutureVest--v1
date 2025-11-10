using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T> (FutureVestContext context) where T: class
    {
        protected readonly FutureVestContext _context = context;
        public async Task <T?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null) return entity;

            return null;
        }

        public async Task<T?> AddAsync(T ent)
        {
           await _context.Set<T>().AddAsync(ent);
            await _context.SaveChangesAsync();
            return ent;
        }

        public async Task<T?> UpdateAsync(int id,T ent)
        {
            var entry = await _context.Set<T>().FindAsync(id);
            if (entry!=null)
            {
                _context.Entry(entry).CurrentValues.SetValues(ent);
                await _context.SaveChangesAsync();
                return ent;
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _context.Set<T>().FindAsync(id);
            if (entry != null)
            {
                _context.Set<T>().Remove(entry);
                await _context.SaveChangesAsync();
            }
        }


        public IQueryable<T> GetAllQuery()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<List<T> >GetAllList()
        { 
            return await _context.Set<T>().ToListAsync();
        }

    }
}
