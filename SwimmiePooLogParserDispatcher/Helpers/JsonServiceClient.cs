using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SwimmiePooLogParserDispatcher.Helpers
{
    public abstract class JsonServiceClient
    {
        private readonly string _serviceUrl;
        protected JsonServiceClient(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        protected TResult Get<TResult>(string endPoint, IEnumerable<KeyValuePair<string, string>> headers = null, params JsonConverter[] converters)
          //  where TResult : class
        {
            var client = new HttpClient { BaseAddress = new Uri(_serviceUrl) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, new List<string> { header.Value });
                }
            }

            var response = client.GetAsync(endPoint).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(response.Content.ReadAsStringAsync().Result, converters);
            }

            throw new Exception(string.Format("Error calling web service. Status Code: {0}", response.StatusCode));
        }

        protected TResult Post<TResult>(string endPoint, dynamic content, IEnumerable<KeyValuePair<string, string>> headers = null, params JsonConverter[] converters)
            //where TResult : class
        {
            var response = PerformPost<dynamic>(endPoint, content, headers);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(response.Content.ReadAsStringAsync().Result, converters);
            }

            throw new Exception(string.Format("Error calling web service. Status Code: {0}", response.StatusCode));
        }

        private HttpResponseMessage PerformPost<TInput>(string endPoint, TInput content, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var client = new HttpClient { BaseAddress = new Uri(_serviceUrl) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, new List<string> { header.Value });
                }
            }

            return client.PostAsJsonAsync(endPoint, content).Result;
        }
    }
}