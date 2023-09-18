using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using Lily.BusinessLogic.IGenericRepository;
using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.UtitlityModel;

namespace TaskManager.BusinessLogic.GenericRepository
{
#nullable disable
    public class GenericRepo<T> : IGenericRepo<T> where T : class
        {
            private readonly ApplicationDbContext _context;
            public readonly DbSet<T> _db;

            public GenericRepo(ApplicationDbContext context)
            {
                _context = context;
                _db = _context.Set<T>();
            }
            public async Task Delete(int id)
            {
                var entity = await _db.FindAsync(id);
                if(entity != null)
                    _db.Remove(entity);
            
                
            }

            public async Task Deleteuser(string id)
            {
                var entity = await _db.FindAsync(id);
                if(entity != null)
                    _db.Remove(entity);
            }
            public void DeleteRange(IEnumerable<T> entities)
            {
                _db.RemoveRange(entities);
            }

            public async Task<T> Get(Expression<Func<T, bool>> expression, List<string>? includes = null)
            {
                IQueryable<T> query = _db;

                if (includes != null)
                {
                    foreach (var IncludeProperties in includes)
                    {
                        query = query.Include(IncludeProperties);
                    }
                }

                return await query.AsNoTracking().FirstOrDefaultAsync(expression);
            }

            public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? Includes = null)
            {
                IQueryable<T> query = _db;

                if (expression != null)
                {
                    query = query.Where(expression);
                }

                if (Includes != null)
                {
                    foreach (var IncludeProperties in Includes)
                    {
                        query = query.Include(IncludeProperties);
                    }
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                return await query.AsNoTracking().ToListAsync();
            }

            public async Task Insert(T entity)
            {
                await _db.AddAsync(entity);
            }

            public async Task InsertRange(IEnumerable<T> entities)
            {
                await _db.AddRangeAsync(entities);
            }

            public void Update(T entity)
            {
                _db.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            public void SaveUpdate()
		    {
			     _context.SaveChangesAsync();
		    }

            public async Task<IPagedList<T>> GetPagedList(RequestParam requestParam, List<string>? includes = null)
            {
                IQueryable<T> query = _db;

                if (includes != null)
                {
                    foreach (var IncludeProperties in includes)
                    {
                        query = query.Include(IncludeProperties);
                    }
                }

                return await query.AsNoTracking().ToPagedListAsync(requestParam.PageNumber, requestParam.pageSize);
            }
        }
    
}
