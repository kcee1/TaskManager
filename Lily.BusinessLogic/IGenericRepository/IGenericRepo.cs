using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.UtitlityModel;
using X.PagedList;

namespace Lily.BusinessLogic.IGenericRepository
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IList<T>> GetAll(
                Expression<Func<T, bool>>? expression = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? Includes = null
            );
        Task<IPagedList<T>> GetPagedList(RequestParam requestParam,
            List<string>? includes = null
            );
        Task<T> Get(Expression<Func<T, bool>>? expression = null, List<string>? includes = null);
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
        Task Delete(int id);
        Task Deleteuser(string id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        void SaveUpdate();
    }
}
