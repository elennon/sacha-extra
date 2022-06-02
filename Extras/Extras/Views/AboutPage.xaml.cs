using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();         
        }


        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var all = await App.Database.GetExtrasAsync();
            foreach (var item in all)
            {
                await App.Database.DeleteExtraAsync(item);
            }
        }
    }
}