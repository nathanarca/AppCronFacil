using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;

namespace AppCronometroFacil.Droid
{
    [Activity(Label = "AppCronometroFacil", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Locked)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-5541916824987072~5851986892");

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
            
        }
        
    }

    
}