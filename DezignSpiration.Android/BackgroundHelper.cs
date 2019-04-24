using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace DezignSpiration.Droid
{
    // This service would help schedule our notifications incase android is killing off all apps
    [Service]
    public class KillStopper : Service
    {
        private AlarmManager serviceStarterAlarmManager;

        public override void OnCreate()
        {
            base.OnCreate();
            Initialize(Application.Context);
            Android.Widget.Toast.MakeText(Application.Context, "BG Service Started", Android.Widget.ToastLength.Long);

            NotificationHelper.SetOrphanedNotifications(Application.Context);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            // Recreate our Notifications before the apps die off
            NotificationHelper.SetOrphanedNotifications(Application.Context);
            Android.Widget.Toast.MakeText(Application.Context, "App is Being Removed", Android.Widget.ToastLength.Long);

            base.OnTaskRemoved(rootIntent);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            // Recreate our Notifications before the apps die off
            Android.Widget.Toast.MakeText(Application.Context, "App is Being Mudered", Android.Widget.ToastLength.Long);
            NotificationHelper.SetOrphanedNotifications(Application.Context);
            base.OnDestroy();
        }

        private void Initialize(Context context)
        {
            Intent intent = new Intent(this, typeof(AlarmManager));
            intent.SetAction(Constants.BACKGROUND_SERVICE_ACTION);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, Constants.BACKGROUND_SERVICE_REQUEST_CODE, intent, 0);
            if (pendingIntent == null)
            {
                Android.Widget.Toast.MakeText(this, "Some problems with creating of PendingIntent", Android.Widget.ToastLength.Long).Show();
            }
            else
            {
                if (serviceStarterAlarmManager == null)
                {
                    serviceStarterAlarmManager = (AlarmManager)GetSystemService(AlarmService);
                    long timeToStart = SystemClock.ElapsedRealtime() + AlarmManager.IntervalFifteenMinutes;
                    serviceStarterAlarmManager.SetRepeating(
                        AlarmType.ElapsedRealtime,
                        timeToStart,
                        AlarmManager.IntervalFifteenMinutes,
                        pendingIntent);
                }
            }
        }
    }
}
