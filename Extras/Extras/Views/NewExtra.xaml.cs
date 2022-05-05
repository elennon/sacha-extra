using Extras.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExtra : ContentPage
    {
        private MemoryStream ms = new MemoryStream();
        private byte[] bytes;
        public NewExtra()
        {
            InitializeComponent();
            BindingContext = new Extra();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            exDate.Date = DateTime.Today;
            //var beachImage = new Image { Aspect = Aspect.AspectFit };
            image2.Source = ImageSource.FromFile("Extras.mice.jpg");
        }

        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
                await copyPic(stream);
            }

            (sender as Button).IsEnabled = true;
        }

        private async Task copyPic(Stream stream)
        {
            await stream.CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            
            var ext = (Extra)BindingContext;
            ext.Name = lugName.Text;
            ext.Description = description.Text;
            ext.Date = exDate.Date;
            ext.Hours = Convert.ToDouble(hours.Text);
            ext.Rate = Convert.ToDouble(rate.Text);
            
            ext.Image = bytes;
            //var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AboutPage)).Assembly;
            //image.SetValue = assembly.GetManifestResourceStream("Extras.mice.jpg");
            
            await App.Database.SaveExtraAsync(ext);
            await DisplayAlert("Alert", "Saved receipt", "OK");
            //// Navigate backwards
            //await Shell.Current.GoToAsync("..");
        }
    }
}