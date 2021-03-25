using System.Collections.Generic;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public interface IHttpClientFactoryService
    {
        Task<List<TResponse>> HttpGetAsync<TResponse>(string uri, string name) where TResponse : class;
        Task<TResponse> HttpGetSingleAsync<TResponse>(string uri, string name) where TResponse : class;

        Task<TResponse> PostAsync<TRequest, TResponse>(TRequest content, string uri, string name)
            where TResponse : class where TRequest : class;

        Task<TResponse> PutAsync<TRequest, TResponse>(TRequest content, string uri, string name)
            where TRequest : class where TResponse : class;

        Task<string> DeleteAsync(string uri, string name);
    }
}