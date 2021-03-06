﻿using System.Threading.Tasks;
using Android.App;
using Android.App.Job;

namespace DezignSpiration.Droid.Jobs
{
    [Service(Name = "com.forefront.dezignspiration.RectifyNotificationJob",
         Permission = "android.permission.BIND_JOB_SERVICE")]
    public class RectifyNotificationJob : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            Task.Run(() =>
            {
                NotificationHelper.SetOrphanedNotifications(Application.Context);
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
