using System;
namespace DezignSpiration.Helpers
{
    public class QuotesAdded
    {
        private static QuotesAdded message;
        public static QuotesAdded Message
        {
            get
            {
                if (message == null)
                {
                    message = new QuotesAdded();
                }
                return message;
            }
        }
    }

    public class NetworkAvailable
    {
        private static NetworkAvailable message;
        public static NetworkAvailable Message
        {
            get
            {
                if (message == null)
                {
                    message = new NetworkAvailable();
                }
                return message;
            }
        }
    }

    public class OnBoardPageChange
    {
        private static OnBoardPageChange message;
        public static OnBoardPageChange Message
        {
            get
            {
                if (message == null)
                {
                    message = new OnBoardPageChange();
                }
                return message;
            }
        }
    }

    public class SwipeToggled
    {
        private static SwipeToggled message;
        public static SwipeToggled Message
        {
            get
            {
                if (message == null)
                {
                    message = new SwipeToggled();
                }
                return message;
            }
        }
    }
}
