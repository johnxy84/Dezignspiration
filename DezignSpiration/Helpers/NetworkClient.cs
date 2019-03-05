using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using DezignSpiration.Interfaces;

namespace DezignSpiration.Helpers
{
    public class NetworkClient : INetworkClient
    {
        //HTTPclient to handle all http calls
        private HttpClient client;

        public NetworkClient()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 512000,
                Timeout = TimeSpan.FromSeconds(5),
                BaseAddress = new Uri(Constants.BASE_URL)
            };
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Post(string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application / json");
            return await client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> Update(string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application / json");
            return await client.PutAsync(url, content);
        }

    }
}
