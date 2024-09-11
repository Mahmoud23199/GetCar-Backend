using GetCar.BL.Const;
using GetCar.BL.DTO.CarDTOs;
using GetCar.BL.GenericRepositry;
using GetCar.DB.ApplicationDbContext;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetCar.BL.Repositry
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly GetCarDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GenericRepositry(GetCarDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context=context;
            _userManager=userManager;
        }
        public async Task AddedAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
           
        }

        public async Task DeleteByIdAsync(Expression<Func<T, bool>> match)
        {
            var item = await GetByIdAsync(match);

            if (item != null)
            {
                _context.Set<T>().Remove(item);
                await _context.SaveChangesAsync();

            }
            else
                throw new KeyNotFoundException("Item not found");
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] includes=null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> match, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query=query.Include(include);
                }
            }
            var item= await query.FirstOrDefaultAsync(match);
            return item;
        }

        public async Task<IEnumerable<T>> GetByNameAsync(Expression<Func<T, bool>> match, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(includes!= null)
            {
                foreach( var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(match).ToListAsync();

            
        }

        public async Task<IEnumerable<T>> OrderItemsAsync(Expression<Func<T, bool>> filter = null,Expression<Func<T, object>> orderBy = null, string orderByDirction = "ASC", string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                if (orderByDirction == OrderBy.Ascending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                    query = query.OrderByDescending(orderBy);
            }
            return await query.ToListAsync();
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity == null) throw new Exception($"{typeof(T).Name} not found");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
