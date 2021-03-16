using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core5_API.Services
{
    public interface IService<TEntity, in TPk> where TEntity : class
    {
        Task<List<TEntity>> GetAsync();

        Task<TEntity> GetAsync(TPk id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TPk id, TEntity entity);

        Task<bool> DeleteAsync(TPk id);
    }
}
