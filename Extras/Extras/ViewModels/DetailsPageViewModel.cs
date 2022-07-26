using System.Windows.Input;
using Extras.Helpers;
using Extras.Views;
using Xamarin.Forms;
namespace Extras.ViewModels
{
    public class DetailsPageViewModel : BasePageViewModel
    {
        public ICommand LogoutCommand
        {
            get;
            private set;
        }
        public DetailsPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LogoutCommand = new Command(() => ResetUserInfo());
        }
        void ResetUserInfo()
        {
            if (UserLogin.passWd != null)
            {
                UserLogin.passWd.Text = "";
            }          
            _navigation.PushAsync(new UserLogin());
            UserSettings.ClearAllData();
        }
    }
}