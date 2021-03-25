using System.Collections.Generic;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public interface IAPIService
    {
        Task<List<TDto>> GetAsync<TEntity, TDto>(bool include = false) where TEntity : class where TDto : class;
        Task<TDto> SingleAsync<TEntity, TDto>(int id, bool include = false) where TEntity : class where TDto : class;
        Task<int> CreateAsync<TEntity, TDto>(TDto item) where TEntity : class where TDto : class;
        Task<bool> UpdateAsync<TEntity, TDto>(TDto item) where TEntity : class where TDto : class;
        Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class;
    }
}