using GetCar.DB.ApplicationDbContext;
using GetCar.DB.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public class CategoryRepositry : ICategoryRepositry
    {
        private readonly GetCarDbContext _context;
        public CategoryRepositry(GetCarDbContext context)
        {
           _context=context;
        }
        public async Task DeleteCategoryAsync(Category category)
        {
            var CarsWithCategore=await _context.Cars.Where(c => c.CategoryID==category.CategoryID).ToListAsync();
            if(CarsWithCategore.Any() )
            {
                foreach(var car in CarsWithCategore)
                {
                    car.CategoryID = null;
                }
            }

           _context.Categories.Remove(category);
           await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category entity)
        {
            try
            {
                var existingEntity = await _context.Categories.FindAsync(entity.GetType().GetProperty("CategoryID").GetValue(entity, null));
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException("Entity not found");
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Update failed: {ex.Message}");
            }
        }
    }
}
