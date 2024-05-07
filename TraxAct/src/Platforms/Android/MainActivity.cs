using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using FirebaseAdmin;

namespace TraxAct
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
		//protected override void OnCreate(Bundle savedInstanceState)
		//{
		//	base.OnCreate(savedInstanceState);

		//	FirebaseApp.InitializeApp(Android.App.Application.Context);

		//	SetContentView(Resource.Layout.activity_main);

		//}
	}
}
