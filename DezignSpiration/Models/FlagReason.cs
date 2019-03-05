using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DezignSpiration.Models
{
    public class FlagReason
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class FlagReasonResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public ObservableRangeCollection<FlagReason> FlagReasons { get; set; }

    }
}
