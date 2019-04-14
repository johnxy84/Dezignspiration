using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using DezignSpiration.Interfaces;
using System.Net.Http.Headers;
using DezignSpiration.Models;

namespace DezignSpiration.Helpers
{
    public class NetworkClient : INetworkClient
    {
        //HTTPclient to handle all http calls
        private readonly HttpClient client;

        public NetworkClient()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 512000,
                Timeout = TimeSpan.FromSeconds(5),
                BaseAddress = new Uri(Constants.BASE_URL),
            };
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            string token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Post(string url, object payload)
        {
            string token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application / json");
            return await client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> Update(string url, object payload)
        {
            string token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application / json");
            return await client.PutAsync(url, content);
        }

        private async Task<string> GetToken()
        {
            string token = Settings.Token;

            if (string.IsNullOrWhiteSpace(token) || (DateTime.Now - Settings.TokenExpiry).Hours > 20)
            {
                // Clear Saved token
                Settings.Token = string.Empty;
                Settings.Token = token = await GetFreshToken();
            }

            return token;
        }

        private async Task<string> GetFreshToken()
        {
            var tokenClient = new HttpClient
            {
                BaseAddress = new Uri(Constants.AUTH_URL)
            };
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                audience = Constants.BASE_URL,
                grant_type = "client_credentials",
                client_secret = Constants.CLIENT_SECRET,
                client_id = Constants.CLIENT_ID
            }), Encoding.UTF8, "application/json");

            var result = await tokenClient.PostAsync("/oauth/token", content);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Token>(response).AccessToken;
            }
            throw new Exception("Unable to get token");
        }

    }
}
