using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace DezignSpiration.Droid
{
    // This service would help schedule our notifications incase android is killing off all apps
    [Service]
	public class KillStopper: Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            // Recreate our Notifications before the apps die off
            NotificationHelper.SetFreshNotifications(Application.Context);
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
            NotificationHelper.SetFreshNotifications(Application.Context);
            base.OnDestroy();
        }
    }
}
