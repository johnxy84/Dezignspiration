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
using Android.Runtime;
using DezignSpiration.Interfaces;
using CarouselView.FormsPlugin.Android;
using Lottie.Forms.Droid;
using System.Collections.Generic;
using Android.App.Job;

[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]

namespace DezignSpiration.Droid
{
    [Activity(Label = "Dezignspiration", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"text/plain")]
    [IntentFilter(new[] { Intent.ActionProcessText }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"text/plain")]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IHelper, IButton
    {

        public static MainActivity Instance { get; private set; }
        private App appInstance;
        private bool doubleBackToExitPressedOnce;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            NotificationHelper.CreateNotificationChannel(Application.Context);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            CarouselViewRenderer.Init();
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AnimationViewRenderer.Init();
            appInstance = new App();
            HandleIntent(Intent);

            if (!InitializeRectifyNotificationJob(Application.Context))
            {
                Utils.LogError(new System.Exception("Error Initializing Android Notification Job"));
            }

            StartService(new Intent(Application.Context, typeof(KillStopper)));
            LoadApplication(appInstance);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent);
        }

        void HandleIntent(Intent intent)
        {
            try
            {
                var receivedText = string.Empty;

                switch (intent.Action)
                {
                    case Intent.ActionSend:
                        receivedText = intent.GetStringExtra(Intent.ExtraText);
                        break;
                    case Intent.ActionProcessText:
                        receivedText = Intent.GetCharSequenceExtra(Intent.ExtraProcessText);
                        break;
                }

                if (!string.IsNullOrWhiteSpace(receivedText))
                {
                    appInstance?.ProcessShareAction(receivedText);
                }

            }
            catch (System.Exception ex)
            {
                Utils.LogError(ex, "ErrorProcessingShareIntent");
            }

        }

        public void DisplayMessage(string title, string message, string positive, string negative, Action<bool> choice)
        {
            AlertDialog.Builder builder = Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1
                ? new AlertDialog.Builder(Instance, Resource.Style.AppCompatDialogStyle)
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

        public void ShowAlert(string message, bool isLongAlert = false, bool isToast = true, string actionMessage = null, Action<object> action = null)
        {
            RunOnUiThread(() =>
            {
                if (isToast)
                {
                    Toast.MakeText(Application.Context, message, isLongAlert ? ToastLength.Long : ToastLength.Short).Show();
                    return;
                }

                View activityRootView = Instance.FindViewById(Android.Resource.Id.Content);
                var snackBar = Snackbar.Make(activityRootView, message, isLongAlert ? 10000 : Snackbar.LengthShort);

                if (!string.IsNullOrWhiteSpace(actionMessage) && action != null)
                {
                    snackBar.SetAction(actionMessage, (obj) =>
                    {
                        snackBar.Dismiss();
                        action?.Invoke(obj);
                    });
                }

                snackBar.Show();

            });
        }

        public void SetScheduledNotifications(List<INotification> notifications)
        {
            NotificationHelper.SetScheduledNotifications(Application.Context, notifications);
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

        public void ShareQuote(DesignQuote quote, bool isLongQuote = false)
        {
            Helper.ShareQuote(Instance, quote, isLongQuote);
        }

        public void ShowOptions(string title, string[] options, Action<object> action = null, string cancelText = "")
        {
            AlertDialog dialog = null;
            AlertDialog.Builder builder = Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1
                ? new AlertDialog.Builder(Instance, Resource.Style.AppCompatDialogStyle)
                : new AlertDialog.Builder(Instance);

            RunOnUiThread(() =>
            {
                if (!IsFinishing)
                {
                    dialog = builder.SetTitle(title)
                        .SetItems(options, (sender, e) =>
                        {
                            action?.Invoke(options[e.Which]);
                        })
                        .SetNegativeButton(cancelText, (s, e) =>
                        {
                            dialog?.Dismiss();
                        })
                        .Show();
                }
            });
        }

        public void BeginSwipeEnableCountdown(double hours)
        {
            NotificationHelper.ScheduleSwipeEnabledNotification(Application.Context, hours);
        }

        public void CancelScheduledNotification(INotification notification)
        {
            if (notification == null) return;
            try
            {
                PendingIntent pendingIntent = NotificationHelper.GetPendingNotificationIntent(notification, Application.Context);

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

        public bool InitializeRectifyNotificationJob(Context context)
        {
            var rectifyNotificationJob = context.CreateJobBuilderUsingJobId<RectifyNotificationJob>(1)
                .SetPeriodic(AlarmManager.IntervalHalfHour)
                .SetPersisted(true)
                .Build();

            var jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
            return jobScheduler.Schedule(rectifyNotificationJob) == JobScheduler.ResultSuccess;
        }

    }
}

