using SQLite;
using DezignSpiration.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using DezignSpiration.Interfaces;
using DezignSpiration.Helpers;
using Newtonsoft.Json;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Linq;

namespace DezignSpiration.Services
{
    public class QuotesRepository : IQuotesRepository
    {
        SQLiteAsyncConnection database;
        private readonly INetworkClient httpClient;

        public QuotesRepository(INetworkClient httpClient)
        {
            database = App.dbConnection;
            this.httpClient = httpClient;
            try
            {
                database.CreateTableAsync<DesignQuote>().Wait();
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorInitializingQuoteTable");
            }
        }

        public async Task<bool> DeleteQuote(DesignQuote designQuote)
        {
            return await database.DeleteAsync(designQuote) == 1;
        }

        public async Task<bool> InsertQuotes(IEnumerable<DesignQuote> quotes)
        {
            try
            {
                await database.InsertAllWithChildrenAsync(quotes);
                return true;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorInsertingQuotes");
                return false;
            }
        }

        public async Task<DesignQuote> GetQuote(int quoteId)
        {
            return await database.GetAsync<DesignQuote>(quoteId);
        }

        public async Task<DesignQuote> GetRandomQuote()
        {
            var sql = $"select * from {nameof(DesignQuote)} order by random() limit 1";
            try
            {
                return await database.FindWithQueryAsync<DesignQuote>(sql);

            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorGettingRandomQuote");
                return null;
            }
        }

        public async Task<List<DesignQuote>> GetAllQuotes()
        {
            try
            {
                return await database.GetAllWithChildrenAsync<DesignQuote>();
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorGettingQuotes");
                return null;
            }
        }

        public async Task<int> CountQuotes()
        {
            return await database.ExecuteScalarAsync<int>($"select count(*) from {typeof(DesignQuote).Name}");
        }

        public async Task<ObservableRangeCollection<DesignQuote>> GetFreshQuotes()
        {
            int totalQuotes = await CountQuotes();
            string flaggedIds = string.Join(",", Settings.FlagedQuoteIds);
            string queryString = $"offset={totalQuotes}&limit={Constants.MAX_FETCH_QUOTE}&status=approved&max_flag_count=0&exclude={flaggedIds}";

            var response = await httpClient.Get($"api/v1/list/quotes?{queryString}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var quotesData = JsonConvert.DeserializeObject<DesignQuoteResponse>(content);

                //Got data, do the needfuls
                if (quotesData != null && quotesData.Data != null)
                {
                    return quotesData.Data.Quotes;
                }
            }
            Utils.LogError(new Exception(content), "RefreshQuotesError");
            return null;
        }

        public async Task<bool> FlagQuote(DesignQuote quote, int flagReasonId)
        {
            try
            {
                var response = await httpClient.Post($"/api/v1/quotes/{quote.Id}/flag", new
                {
                    flag_reason = flagReasonId
                });

                if (!response.IsSuccessStatusCode)
                {
                    var responseMessae = await response.Content.ReadAsStringAsync();
                    Utils.LogError(new Exception(responseMessae), "ErrorFlaggingQuote");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorFlagingQuote");
                return false;
            }
        }

        public async Task<bool> AddQuote(DesignQuote quote, bool isAnonymous, string deviceId = null)
        {
            var author = isAnonymous || string.IsNullOrWhiteSpace(quote.Author) ? null : quote.Author;

            var response = await httpClient.Post("/api/v1/quotes", new
            {
                color = quote.Color.Id,
                quote = quote.Quote,
                author,
                device_id = deviceId
            });
            return response.IsSuccessStatusCode;
        }
    }
}
