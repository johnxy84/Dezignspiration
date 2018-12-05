using System;
namespace DezignSpiration.Models
{
    public class ScheduledNotification
    {
        public DesignQuote DesignQuote { get; set; }

        public NotificationType NotificationType { get; set; }

        public TimeSpan TimeSpan { get; set; }
        
    }
}
