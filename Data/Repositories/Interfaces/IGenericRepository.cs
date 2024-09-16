using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Data
{
    public interface  IGenericRepository<TEntity>
    {

        public Task<List<TEntity>> GetAllAsync();
        public Task<TEntity> GetAsync(int id);
        /// <summary> 
        /// Perform a divide by b
        /// <para>line1</para>
        /// </summary>
        /// <returns>int value</returns>
        public Task<TEntity> AddAsync(TEntity entity);
        public TEntity Update(TEntity entity);
        public Task DeleteAsync(int id);
    }
}
