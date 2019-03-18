using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace DezignSpiration.Droid
{
    [Activity(
        Theme = "@style/MainTheme.Splash",
        Label = "Dezignspiration",
        ScreenOrientation = ScreenOrientation.Portrait,
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}