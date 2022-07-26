using System.ComponentModel;
using System.Runtime.CompilerServices;
using Extras.Helpers;
using Xamarin.Forms;

namespace Extras.ViewModels
{
    public class BasePageViewModel : INotifyPropertyChanged
    {
        public INavigation _navigation;
        public string UserName
        {
            get => UserSettings.UserName;
            set
            {
                UserSettings.UserName = value;
                NotifyPropertyChanged("UserName");
            }
        }

        public string IsLoggedIn
        {
            get => UserSettings.IsLoggedIn;
            set
            {
                UserSettings.IsLoggedIn = value;
                NotifyPropertyChanged("IsLoggedIn");
            }
        }
        public string MobileNumber
        {
            get => UserSettings.MobileNumber;
            set
            {
                UserSettings.MobileNumber = value;
                NotifyPropertyChanged("MobileNumber");
            }
        }
        public string Email
        {
            get => UserSettings.Email;
            set
            {
                UserSettings.Email = value;
                NotifyPropertyChanged("Email");
            }
        }
        public string WrongPassword { get; set; }
        
        public string Password
        {
            get => UserSettings.Password;
            set
            {
                UserSettings.Password = value;
                NotifyPropertyChanged("Password");
            }
        }
        #region INotifyPropertyChanged  
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion      
    }
}
