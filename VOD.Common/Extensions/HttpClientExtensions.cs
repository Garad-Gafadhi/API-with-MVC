using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VOD.Common.Exceptions;

namespace VOD.Common.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<List<TResponse>> GetAsyncExs<TResponse>(this HttpClient client, string uri)
        {
            var requestMessage = uri.CreateRequestHeaders(HttpMethod.Get);
            using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();
            await response.CheckStatusCodes();
            return stream.ReadAndDeserializeFromJson<List<TResponse>>();
        }

        public static async Task<TResponse> GetAsyncEx<TResponse, TRequest>(this HttpClient httpClient, string uri,
            TRequest content)
        {
            var requestMessage = uri.CreateRequestHeaders(HttpMethod.Get);
            if (content != null) await requestMessage.CreateRequestContent(content);

            using var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();
            await response.CheckStatusCodes();
            return stream.ReadAndDeserializeFromJson<TResponse>();
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
            TRequest content)
        {
            using var requestMessage = uri.CreateRequestHeaders(HttpMethod.Post);
            using ((await requestMessage.CreateRequestContent(content)).Content)
            {
                using var responseMessage = await httpClient.SendAsync(requestMessage);
                await responseMessage.CheckStatusCodes();
                return await responseMessage.DeserializeResponse<TResponse>();
            }
        }

        public static async Task<TResponse> PutAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
            TRequest content)
        {
            using var requestMessage = uri.CreateRequestHeaders(HttpMethod.Put);
            using ((await requestMessage.CreateRequestContent(content)).Content)
            {
                using var responseMessage =
                    await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
                await responseMessage.CheckStatusCodes();
                return await responseMessage.DeserializeResponse<TResponse>();
            }
        }

        public static async Task<string> RemoveAsync(this HttpClient httpClient, string uri)
        {
            using var requestMessage = uri.CreateRequestHeaders(HttpMethod.Delete);
            using var responseMessage =
                await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            await responseMessage.CheckStatusCodes();

            return await responseMessage.Content.ReadAsStringAsync();
        }


        #region helper methods

        private static HttpRequestMessage CreateRequestHeaders(this string uri, HttpMethod httpMethod)
        {
            var requestHeader = new HttpRequestMessage(httpMethod, uri);
            requestHeader.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (httpMethod.Equals(HttpMethod.Get))
                requestHeader.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            return requestHeader;
        }


        private static async Task CheckStatusCodes(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                object validationErrors = null;
                var message = string.Empty;
                if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    var errorStream = await response.Content.ReadAsStreamAsync();
                    validationErrors = errorStream.ReadAndDeserializeFromJson();
                    message = "Cannot process the sh*t";
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    message = "Bad request";
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    message = "Access denied";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    message = " Not logged in";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    message = " Could Not Found Entity";
                }

                throw new HttpResponseException(response.StatusCode, message, validationErrors);
            }

            response.EnsureSuccessStatusCode();
        }

        public static object ReadAndDeserializeFromJson(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead) throw new NotSupportedException("Cannot read from this stream");

            using var streamReader = new StreamReader(stream, new UTF8Encoding(), true, 1024, false);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize(jsonTextReader);
        }

        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead) throw new NotSupportedException("Cannot read from this stream");

            using var streamReader = new StreamReader(stream, new UTF8Encoding(), true, 1024, false);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<T>(jsonTextReader);
        }

        private static async Task<HttpRequestMessage> CreateRequestContent<TRequest>(
            this HttpRequestMessage requestMessage, TRequest content)
        {
            requestMessage.Content = await content.SerializeRequestContentAsync();
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return requestMessage;
        }

        private static async Task<StreamContent> SerializeRequestContentAsync<TRequest>(this TRequest content)
        {
            var stream = new MemoryStream();
            await stream.SerializeToJsonAndWriteAsync(content, new UTF8Encoding(), 1024, true);
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamContent(stream);
        }

        private static async Task<TResponse> DeserializeResponse<TResponse>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();
            return responseStream.ReadAndDeserializeFromJson<TResponse>();
        }

        #endregion
    }
}