
using Extras.Views;
using Xamarin.Forms;

namespace Extras
{
    public partial class AppShell : Shell
    {
        
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CloseUp), typeof(CloseUp));
            Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));
            Routing.RegisterRoute(nameof(UserLogin), typeof(UserLogin));
        }

    }
}
