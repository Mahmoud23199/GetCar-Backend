using GetCar.BL.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.GenericRepositry
{
    public interface IGenericRepositry<T>where T : class
    {
        Task AddedAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> match, string[] includes = null);
        Task<IEnumerable<T>> GetByNameAsync(Expression<Func<T, bool>> match, string[] includes = null);

        Task<IEnumerable<T>> GetAllAsync(string[] includes);

        Task<T>UpdateAsync(int id, T entity);


        //Task DeleteByNameAsync(Expression<Func<T, bool>> match);
        Task DeleteByIdAsync(Expression<Func<T, bool>> match);

        Task<IEnumerable<T>> OrderItemsAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, string orderByDirction = OrderBy.Ascending, string[] includes = null);



    }
}
