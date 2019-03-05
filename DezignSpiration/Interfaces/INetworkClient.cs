using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DezignSpiration.Interfaces
{
    public interface INetworkClient
    {
        Task<HttpResponseMessage> Get(string url);

        Task<HttpResponseMessage> Post(string url, object payload);

        Task<HttpResponseMessage> Update(string url, object payload);
    }
}
