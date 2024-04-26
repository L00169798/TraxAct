using TraxAct.Services;
using TraxAct.Views;


namespace TraxAct
{
	public partial class App : Application
	{

		public App()
		{

			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxcd3VTQmleVEd1W0s=");
			InitializeComponent();


			var dbContext = new MyDbContext();
			var services = new ServiceCollection();
			services.AddSingleton<UserService>();

			var serviceProvider = services.BuildServiceProvider();

			var userService = serviceProvider.GetService<UserService>();

			MainPage = new AppShell(userService);
			MainPage.Navigation.PushAsync(new SignInPage());
		}
	}
}