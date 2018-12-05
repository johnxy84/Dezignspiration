using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace DezignSpiration.Helpers
{
    public class Client
    {
        //HTTPclient to handle all http calls
        private HttpClient _client;

        public Client(string baseUrl)
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 512000,
                Timeout = TimeSpan.FromSeconds(5),

                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Post(string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application / json");
            return await _client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> Update(string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application / json");
            return await _client.PutAsync(url, content);
        }

    }
}
