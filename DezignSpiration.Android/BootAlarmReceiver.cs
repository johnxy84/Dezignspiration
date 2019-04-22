using Android.App;
using Android.Content;

namespace DezignSpiration.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionMyPackageReplaced })]
    public class BootAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case Intent.ActionBootCompleted:
                case Intent.ActionReboot:
                case Intent.ActionMyPackageReplaced:
                    NotificationHelper.SetFreshNotifications(context);
                    break;
            }
        }

    }
}
