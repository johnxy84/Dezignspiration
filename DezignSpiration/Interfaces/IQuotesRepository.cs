using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DezignSpiration.Models;

namespace DezignSpiration.Interfaces
{
    public interface IQuotesRepository
    {
        Task<bool> DeleteQuote(DesignQuote designQuote);

        Task<bool> InsertQuotes(IEnumerable<DesignQuote> quotes);

        Task<DesignQuote> GetQuote(int quoteId);

        Task<List<DesignQuote>> GetAllQuotes();

        Task<int> CountQuotes();

        Task<ObservableRangeCollection<DesignQuote>> GetFreshQuotes();

        Task<bool> FlagQuote(DesignQuote quote, int flagReasonId);

        Task<bool> AddQuote(DesignQuote quote, string deviceId = null);

    }
}
