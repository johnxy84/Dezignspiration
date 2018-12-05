using System;

using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using DezignSpiration.Helpers;
using DezignSpiration.Droid;
using DezignSpiration.Models;
using Android.Content;
using Java.Lang;
using Android.Support.Design.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]

namespace DezignSpiration.Droid
{
    [Activity(Label = "ApplicationName", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IHelper
    {

        public static MainActivity Instance { get; private set; }
        private bool doubleBackToExitPressedOnce;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            NotificationHelper.CreateNotificationChannel(Application.Context);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            StartService(new Intent(Application.Context, typeof(KillStopper)));
            LoadApplication(new App());
        }

        public void DisplayMessage(string title, string message, string positive, string negative, Action<bool> choice)
        {
            AlertDialog.Builder builder = Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1
                ? new AlertDialog.Builder(Instance, Android.Resource.Style.ThemeDeviceDefaultDialogAlert)
                : new AlertDialog.Builder(Instance);

            RunOnUiThread(() =>
            {
                if (!IsFinishing)
                {
                    builder.SetTitle(title)
                         .SetMessage(message)
                         .SetPositiveButton(positive, (sender, e) =>
                         {
                             choice(true);
                         })
                         .SetNegativeButton(negative, (sender, e) =>
                         {
                             choice(false);
                         })
                         .SetIcon(Android.Resource.Drawable.IcDialogAlert)
                         .Show();
                }
            });
        }

        public void ShowAlert(string message, bool isLongAlert = true, bool isToast = true, string actionMessage = null, Action<object> action = null)
        {
            RunOnUiThread(() =>
            {
                if(isToast)
                {
                    Toast.MakeText(Application.Context, message, isLongAlert ? ToastLength.Long : ToastLength.Short).Show();
                    return;
                }

                View activityRootView = Instance.FindViewById(Android.Resource.Id.Content);
                var snackBar = Snackbar.Make(activityRootView, message, isLongAlert ? Snackbar.LengthLong : Snackbar.LengthShort);

                if (!string.IsNullOrWhiteSpace(actionMessage) && action != null)
                {
                    snackBar.SetAction(actionMessage, (obj) =>
                    {
                        snackBar.Dismiss();
                        action?.Invoke(obj);
                    });
                }

                snackBar.Show();

                //Toast.MakeText(Instance, message, isLongAlert? ToastLength.Long: ToastLength.Short).Show();
            });
        }

        public void ScheduleNotification(ScheduledNotification scheduledNotification)
        {
            NotificationHelper.ScheduledNotification(Application.Context, scheduledNotification);
        }

        public void CancelScheduledNotification(ScheduledNotification scheduledNotification)
        {
            if (scheduledNotification == null) return;
            try
            {
                PendingIntent pendingIntent = NotificationHelper.GetPendingNotificationIntent(scheduledNotification, Application.Context);

                if (pendingIntent != null)
                {
                    AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(AlarmService);
                    alarmManager?.Cancel(pendingIntent);
                }
            }
            catch (System.Exception ex)
            {
                Utils.LogError(ex, "CancelScheduledNotification");
            }
        }

        public void SetScheduledNotifications()
        {
            NotificationHelper.SetScheduledNotifications(Application.Context);
        }

        public void OpenUrl(string url)
        {
            Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            Instance?.StartActivity(browserIntent);
        }

        public void BackButtonPressed()
        {
            if (doubleBackToExitPressedOnce)
            {
                JavaSystem.Exit(0);
                return;
            }

            doubleBackToExitPressedOnce = true;
            Toast.MakeText(Instance, "Press back again to Leave", ToastLength.Short).Show();
            new Handler().PostDelayed(() => { doubleBackToExitPressedOnce = false; }, 2000);
        }

        public void ShareQuote(DesignQuote quote)
        {
            Helper.ShareQuote(Instance, quote);
        }

        public void ShowOptions(string title, string[] options, Action<object> action = null)
        {
            AlertDialog.Builder builder = Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1
                ? new AlertDialog.Builder(Instance, Android.Resource.Style.ThemeDeviceDefaultDialogAlert)
                : new AlertDialog.Builder(Instance);

            RunOnUiThread(() =>
            {
                if (!IsFinishing)
                {
                    builder.SetTitle(title)
                       .SetItems(options, (sender, e) =>
                       {
                            action?.Invoke(options[e.Which]);
                       })
                       .Show();
                }
            });
        }
    }
}

