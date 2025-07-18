using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<(HttpResponseMessage?, TOutPut?)> Post<TOutPut>(
            string route,
            object payload
            ) where TOutPut : class
        {
            var response = await _httpClient.PostAsync(
                route,
                new StringContent(
                JsonSerializer.Serialize(payload),
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
                JsonSerializer.Serialize(payload),
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
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   }
                 );
            return output;
        }
        private string PrepareGetRoute(string route, object? parameters)
        {
            if (parameters is null)
                return route;

            var parameterJson = JsonSerializer.Serialize(parameters);
            var parametersDictionary = Newtonsoft.Json.JsonConvert
                .DeserializeObject<Dictionary<string, string>>(parameterJson);
            return QueryHelpers.AddQueryString(route, parametersDictionary!);
        }

    }
}
