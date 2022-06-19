
using Extras.Data;
using Extras.Models;
using Extras.Views;
using System;
using System.IO;
using System.Threading.Tasks;
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
            bool IsUserLoggedIn = Current.Properties.ContainsKey("IsLoggedIn") ? Convert.ToBoolean(Current.Properties["IsLoggedIn"]) : false;

            if (!IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                //MainPage = new NavigationPage(new Extras.ProjectsPage());
                MainPage = new AppShell();
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
