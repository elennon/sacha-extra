
using Extras.Data;
using System;
using System.IO;
using System.Threading.Tasks;
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
