using Microsoft.Maui.Controls;
using TraxAct.Views;

namespace TraxAct
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(TimetablePage), typeof(TimetablePage));
        }


    }
}
