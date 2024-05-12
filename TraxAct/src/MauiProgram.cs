using Microsoft.Extensions.Logging;
using TraxAct.Services;
using Syncfusion.Maui.Core.Hosting;
using Firebase.Auth;
using Firebase.Auth.Providers;
using TraxAct.ViewModels;

namespace TraxAct
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.ConfigureSyncfusionCore();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

			builder.Services.AddTransient<SignInViewModel>();

			builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
			{
				ApiKey = "AIzaSyBCmctzgS7IOUNUKnorKAEpezbSaWrRL_Y",
				AuthDomain = "traxact-c3d95.firebaseapp.com",
				Providers = new FirebaseAuthProvider[]
			{
				new EmailProvider()
			}
			}));
	#if DEBUG
			builder.Logging.AddDebug();
	#endif
			builder.Services.AddSingleton<MyDbContext>();
			builder.Services.AddSingleton<UserService>();
			return builder.Build();
		}
	}
}
