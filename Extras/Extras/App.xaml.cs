
using Extras.Data;
using Extras.Helpers;
using Extras.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Extras
{
    public partial class App : Application
    {
        static ExtrasDatabase database;
        //public static bool IsUserLoggedIn { get; set; }
        //Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;

        // Create the database connection as a singleton.
        public static ExtrasDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ExtrasDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Extras.db3"));
                }
                return database;
            }
        }
        
        public App()
        {
            InitializeComponent();
            var Foo = UserSettings.IsLoggedIn;

            if (Foo == Boolean.FalseString)
            {
                MainPage = new AppShell();
                GoToLogin();
                //MainPage = new NavigationPage(new UserLogin());
            }
            else
            {
                MainPage = new AppShell();
                //MainPage = new NavigationPage(new UserLogin());
            }
        }

        private async void GoToLogin()
        {
            await Shell.Current.GoToAsync($"{nameof(UserLogin)}");
        }

        private async Task<string> GetPw(string key)
        {
            string oauthToken = null;
            try
            {
                oauthToken = await SecureStorage.GetAsync(key);
            }
            catch (Exception)
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
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
        protected  override void OnStart()
        {
            //await clearDb();
        }

        //private async Task clearDb()
        //{
        //    var extrs = await database.GetExtrasAsync();
        //    foreach (var item in extrs)
        //    {
        //        await database.DeleteExtraAsync(item);
        //        var pics = await database.GetPicsAsync(item.ID);
        //        foreach (var pic in pics)
        //        {
        //            await database.DeletePicAsync(pic);
        //        }
        //    }
            
        //}

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
