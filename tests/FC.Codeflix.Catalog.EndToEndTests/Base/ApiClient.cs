using FC.Codeflix.Catalog.Api.Configurations.Polices;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _defaultSerializerOptions;

        public ApiClient(HttpClient httpClient)
        {

            _httpClient = httpClient;
            _defaultSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<(HttpResponseMessage?, TOutPut?)> Post<TOutPut>(
            string route,
            object payload
            ) where TOutPut : class
        {
            var response = await _httpClient.PostAsync(
                route,
                new StringContent(
                JsonSerializer.Serialize(
                    payload,
                _defaultSerializerOptions),
                Encoding.UTF8,
                "application/json")
             );
            var output = await GetOutput<TOutPut>(response);
            return (response, output);
        }

        public async Task<(HttpResponseMessage?, TOutPut?)> Get<TOutPut>(string route,
            object? parameters = null)
            where TOutPut : class
        {
            var url = PrepareGetRoute(route, parameters);
            var response = await _httpClient.GetAsync(url);
            TOutPut? output = await GetOutput<TOutPut>(response); ;
            return (response, output);
        }


        public async Task<(HttpResponseMessage?, TOutPut?)> Put<TOutPut>(
                      string route,
                      object payload
         ) where TOutPut : class
        {
            var response = await _httpClient.PutAsync(
                route,
                new StringContent(
                JsonSerializer.Serialize(
                 payload,
                _defaultSerializerOptions),
                Encoding.UTF8,
                "application/json")
             );
            TOutPut? output = await GetOutput<TOutPut>(response);
            return (response, output);
        }


        public async Task<(HttpResponseMessage?, TOutPut?)> Delete<TOutPut>(string route)
         where TOutPut : class
        {
            var response = await _httpClient.DeleteAsync(route);
            TOutPut? output = await GetOutput<TOutPut>(response);
            return (response, output);
        }


        private async Task<TOutPut?> GetOutput<TOutPut>(HttpResponseMessage response)
            where TOutPut : class
        {
            var outputString = await response.Content.ReadAsStringAsync();
            TOutPut? output = null;
            if (!string.IsNullOrWhiteSpace(outputString))
                output = JsonSerializer.Deserialize<TOutPut>(
                   outputString,
                  _defaultSerializerOptions
                 );
            return output;
        }
        private string PrepareGetRoute(string route, object? parameters)
        {
            if (parameters is null)
                return route;

            var parameterJson = JsonSerializer.Serialize(parameters, _defaultSerializerOptions);
            var parametersDictionary = Newtonsoft.Json.JsonConvert
                .DeserializeObject<Dictionary<string, string>>(parameterJson);
            return QueryHelpers.AddQueryString(route, parametersDictionary!);
        }

    }
}
