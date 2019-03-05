using System.Threading.Tasks;
using DezignSpiration.Models;
namespace DezignSpiration.Interfaces
{
    public interface IFlagReasonService
    {
        Task<ObservableRangeCollection<FlagReason>> GetFlagReasons();
    }
}
