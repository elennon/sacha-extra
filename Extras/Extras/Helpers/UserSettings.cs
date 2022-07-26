using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Extras.Helpers
{
    public static class UserSettings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static string IsLoggedIn
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLoggedIn), Boolean.FalseString);
            set => AppSettings.AddOrUpdateValue(nameof(IsLoggedIn), value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }
        public static string MobileNumber
        {
            get => AppSettings.GetValueOrDefault(nameof(MobileNumber), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(MobileNumber), value);
        }
        public static string Email
        {
            get => AppSettings.GetValueOrDefault(nameof(Email), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Email), value);
        }
        public static string Password { get; set; }
        //{
        //    // => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
        //    //set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        //}
        
        public static string WrongPassword { get; set; }
        
        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

    }
}
