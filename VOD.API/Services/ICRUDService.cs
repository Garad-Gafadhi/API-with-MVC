using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VOD.Database.Service
{
    public interface ICrudService
    {
        Task<List<TDto>> GetAsync<TEntity, TDto>(bool include = false) where TEntity : class where TDto : class;

        Task<TDto> GetSingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class where TDto : class;

        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class;

        Task<int> CreateAsync<TDto, TEntity>(TDto item) where TDto : class where TEntity : class;
        Task<bool> UpdateAsync<TDto, TEntity>(TDto item) where TDto : class where TEntity : class;

        Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class;
    }
}