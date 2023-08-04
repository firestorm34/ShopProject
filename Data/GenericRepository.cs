using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace ShopProject.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity: class
    {
        DbContext context;
        public GenericRepository(DbContext _context)
        {
            context = _context;
        }
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            return entity;

        }

        public virtual async Task Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if(entity == null)
            {
                return ;
            }
            context.Remove(entity);
            return ;
        }


        public virtual async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual  TEntity Update(TEntity entity)
        {

            context.Update(entity);

            //context.Attach(entity);
            //context.Entry<TEntity>(entity).State = EntityState.Modified;
            return entity;


        }
    }
}
