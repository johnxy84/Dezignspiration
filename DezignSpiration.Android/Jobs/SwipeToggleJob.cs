using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using DezignSpiration.Helpers;

namespace DezignSpiration.Droid.Jobs
{
    [Service(Name = "com.forefront.dezignspiration.SwipeToggleJob",
         Permission = "android.permission.BIND_JOB_SERVICE")]
    public class SwipeToggleJob : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            Task.Run(() =>
            {
                Xamarin.Forms.MessagingCenter.Send(SwipeToggled.Message, Helpers.Constants.SWIPE_TOGGLED, true);
                NotificationHelper.SendSwipeEnabledNotification(Application.Context);
                JobFinished(@params, false);
            });
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            return false;
        }
    }
}
