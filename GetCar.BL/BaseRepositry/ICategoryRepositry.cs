using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public interface ICategoryRepositry
    {
        Task DeleteCategoryAsync(Category category);
        Task UpdateAsync(Category entity);
    }
}
