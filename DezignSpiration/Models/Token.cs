using System;
using Newtonsoft.Json;

namespace DezignSpiration.Models
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
