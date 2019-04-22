using System;
using System.Threading.Tasks;
using DezignSpiration.Models;

namespace DezignSpiration.Interfaces
{
    public interface INotification
    {
        NotificationType GetNotificationType();

        TimeSpan TimeToSend();

        void ToggleNotificationIsSet(bool isNotificationSet);

        bool ShouldCreateNotification();

        int GetNotificationId();

        Task<DesignQuote> GetDesignQuote(IQuotesRepository quotesRepository);

        void ClearNotification();
    }
}
