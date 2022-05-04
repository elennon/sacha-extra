
using Extras.Data;
using System;
using System.IO;
using Xamarin.Forms;

namespace Extras
{
    public partial class App : Application
    {
        static ExtrasDatabase database;
        //static ReceiptsDatabase rdb;

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
            MainPage = new AppShell();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
