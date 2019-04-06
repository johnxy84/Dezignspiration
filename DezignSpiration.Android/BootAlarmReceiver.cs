using System;
using Android.App;
using Android.Content;

namespace DezignSpiration.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBootCompleted || intent.Action == Intent.ActionReboot)
            {
                NotificationHelper.SetFreshNotifications(context);
            }
        }

    }
}
