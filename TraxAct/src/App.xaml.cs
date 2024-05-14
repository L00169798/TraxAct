using TraxAct.Services;


namespace TraxAct
{
    public partial class App : Application
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            //Registering Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxcd3VTQmleVEd1W0s=");
            InitializeComponent();

            //Create an instance of UserService and pass it to the AppShell
			var userService = new UserService();
			MainPage = new AppShell(userService);
		}
    }
}