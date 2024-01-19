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
        public GenericRepository(DbContext context)
        {
            this.context = context;
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            return entity;

        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if(entity == null)
            {
                return ;
            }
            context.Remove(entity);
            return ;
        }


        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
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
