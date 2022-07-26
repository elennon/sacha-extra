using Extras.Helpers;
using Extras.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Extras.ViewModels
{
    public class UserLoginPageViewModel : BasePageViewModel
    {      
        public ICommand LoginCommand
        {
            get;
            private set;
        }
        public UserLoginPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoginCommand = new Command(() => UpdateUserInfo());
        }
        async void UpdateUserInfo()
        {
            var isfirst = GetPw("FirstTimeLogin").Result;
            if (isfirst == null)
            {
                SetPw("Password", UserSettings.Password).Wait();
                SetPw("FirstTimeLogin", Boolean.FalseString).Wait();
            }
            var pw = GetPw("Password").Result;
            if (pw != UserSettings.Password)
            {
                UserLogin.label.Text = "wrong bloody password";
            }
            else 
            {
                UserLogin.label.Text = "";
                //await _navigation.PushAsync(new ProjectsPage()); 
                //await Shell.Current.GoToAsync($"{nameof(CloseUp)}?{nameof(CloseUp.ID)}={"hhh"}");
                await Shell.Current.Navigation.PopToRootAsync();
                //await Shell.Current.GoToAsync($"{nameof(ProjectsPage)}");
                //await .Navigation.PushModalAsync(new ProjectsPage());PopToRootAsync 
            }

        }
        private async Task<string> GetPw(string key)
        {
            string oauthToken = null;
            try
            {
                oauthToken = await SecureStorage.GetAsync(key);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
            return oauthToken;
        }
        private async Task SetPw(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
    }
}