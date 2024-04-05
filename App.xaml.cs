using TraxAct.Services;
using SQLite;
using Microsoft.EntityFrameworkCore;


namespace TraxAct
{
    public partial class App : Application
    {
        public App()
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxcd3VTQmleVEd1W0s=");
            InitializeComponent();


			var dbContext = new MyDbContext();

            MainPage = new AppShell();
        }
    }
}