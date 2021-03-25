using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VOD.Common.Exceptions;
using VOD.Common.Extension;
using VOD.Common.Extensions;

namespace VOD.Common.Services
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        #region Properties

        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Constructors

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #endregion


        #region Methods

        public async Task<List<TResponse>> HttpGetAsync<TResponse>(string uri, string name) where TResponse : class
        {
            if (uri.IsEmpty() || name.IsEmpty())
                throw new HttpResponseException(HttpStatusCode.NotFound, "Can not find the sh*t");

            var httpClient = _httpClientFactory.CreateClient(name);
            return await httpClient.GetAsyncExs<TResponse>(uri.ToLower());
        }

        public async Task<TResponse> HttpGetSingleAsync<TResponse>(string uri, string name) where TResponse : class
        {
            if (uri.IsEmpty() || name.IsEmpty())
                throw new HttpResponseException(HttpStatusCode.NotFound, "Can not find the sh*t");

            var httpClient = _httpClientFactory.CreateClient(name);
            return await httpClient.GetAsyncEx<TResponse, string>(uri.ToLower(), null);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest content, string uri, string name)
            where TRequest : class where TResponse : class
        {
            if (uri.IsEmpty() || name.IsEmpty())
                throw new HttpResponseException(HttpStatusCode.NotFound, "Can not find the sh*t");
            var httpClient = _httpClientFactory.CreateClient(name);
            return await httpClient.PostAsync<TRequest, TResponse>(uri.ToLower(), content);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(TRequest content, string uri, string name)
            where TRequest : class where TResponse : class
        {
            if (uri.IsEmpty() || name.IsEmpty())
                throw new HttpResponseException(HttpStatusCode.NotFound, "Can not find the sh*t");

            var httpClient = _httpClientFactory.CreateClient(name);

            return await httpClient.PutAsync<TRequest, TResponse>(uri.ToLower(), content);
        }

        public async Task<string> DeleteAsync(string uri, string name)
        {
            if (uri.IsEmpty() || name.IsEmpty())
                throw new HttpResponseException(HttpStatusCode.NotFound, "Can not find the sh*t");

            var httpClient = _httpClientFactory.CreateClient(name);

            return await httpClient.RemoveAsync(uri.ToLower());
        }

        #endregion
    }
}