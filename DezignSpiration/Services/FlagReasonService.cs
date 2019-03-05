using System;
using System.Threading.Tasks;
using DezignSpiration.Interfaces;
using DezignSpiration.Models;
using Newtonsoft.Json;
using DezignSpiration.Helpers;

namespace DezignSpiration.Services
{
    public class FlagService : IFlagReasonService
    {
        private readonly INetworkClient httpClient;
        private readonly IQuotesRepository quotesRepository;

        public FlagService(INetworkClient httpClient, IQuotesRepository quotesRepository)
        {
            this.httpClient = httpClient;
            this.quotesRepository = quotesRepository;
        }

        public async Task<ObservableRangeCollection<FlagReason>> GetFlagReasons()
        {
            if (!Settings.ShouldRefreshFlagReasons && Settings.FlagReasons.Count != 0)
            {
                return Settings.FlagReasons;
            }

            var response = await httpClient.Get("/api/v1/list/flag_reasons");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var flagReasonsResponse = JsonConvert.DeserializeObject<FlagReasonResponse>(content);
                Settings.FlagReasons = flagReasonsResponse.FlagReasons;
                return Settings.FlagReasons;
            }
            Utils.LogError(new Exception(content), "ErrorFetchingFlagreasons");
            return null;
        }
    }
}
