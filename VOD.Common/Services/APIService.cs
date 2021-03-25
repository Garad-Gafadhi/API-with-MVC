using System.Collections.Generic;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public class APIService : IAPIService
    {
        #region Constructors

        public APIService(IHttpClientFactoryService factoryService)
        {
            _factoryService = factoryService;
        }

        #endregion

        #region Methods

        public async Task<List<TDTO>> GetAsync<TEntity, TDTO>(bool include = false)
            where TEntity : class where TDTO : class
        {
            var uri = GetUri<TEntity>();
            return await _factoryService.HttpGetAsync<TDTO>($"{uri}?include={include}", "AdminClient");
        }

        public async Task<TDto> SingleAsync<TEntity, TDto>(int id, bool include = false)
            where TEntity : class where TDto : class
        {
            var uri = GetUri<TEntity>(id);
            var result =
                await _factoryService.HttpGetSingleAsync<TDto>($"{uri}?include={include}", "AdminClient");
            return result;
        }

        public async Task<int> CreateAsync<TEntity, TDto>(TDto item) where TEntity : class where TDto : class
        {
            var uri = GetUri<TEntity>();
            var response = await _factoryService.PostAsync<TDto, TDto>(item, uri, "AdminClient");
            return (int) response.GetType().GetProperty("Id").GetValue(response);
        }

        public async Task<bool> UpdateAsync<TEntity, TDto>(TDto item) where TEntity : class where TDto : class
        {
            try
            {
                var id = (int) item.GetType().GetProperty("Id").GetValue(item);
                var uri = GetUri<TEntity>(id);
                var response = await _factoryService.PutAsync<TDto, TDto>(item, uri, "AdminClient");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class
        {
            try
            {
                var uri = GetUri<TEntity>(id);

                var response = await _factoryService.DeleteAsync(uri, "AdminClient");
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Properties

        private string GetUri<TEntity>()
        {
            return $"api/{typeof(TEntity).Name}s";
        }

        private string GetUri<TEntity>(int id)
        {
            return $"api/{typeof(TEntity).Name}s/{id}";
        }

        private readonly IHttpClientFactoryService _factoryService;

        #endregion
    }
}