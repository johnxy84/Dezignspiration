using Foundation;
using UIKit;
using Lottie.Forms.iOS.Renderers;
using DezignSpiration.Interfaces;
using DezignSpiration.Models;
using System;
using CarouselView.FormsPlugin.iOS;
using DezignSpiration.iOS;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(AppDelegate))]


namespace DezignSpiration.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IHelper
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xamarin.Forms.Forms.Init();
            AnimationViewRenderer.Init();
            CarouselViewRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public void BeginSwipeEnableCountdown(int hours = 12)
        {
            throw new NotImplementedException();
        }

        public void CancelScheduledNotification(INotification notificationData)
        {
            throw new NotImplementedException();
        }

        public void DisplayMessage(string title, string message, string positive, string negative, Action<bool> choice)
        {
            throw new NotImplementedException();
        }

        public void SetScheduledNotifications(List<INotification> notifications)
        {
            throw new NotImplementedException();
        }

        public void ShareQuote(DesignQuote quote, bool isLongQuote = false)
        {
            throw new NotImplementedException();
        }

        public void ShowAlert(string message, bool isLongAlert = false, bool isToast = true, string actionMessage = null, Action<object> action = null)
        {
            throw new NotImplementedException();
        }

        public void ShowOptions(string title, string[] options, Action<object> choice, string cancelText = "Cancel")
        {
            throw new NotImplementedException();
        }
    }
}
