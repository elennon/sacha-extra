using Extras.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExtra : ContentPage
    {
        public NewExtra()
        {
            InitializeComponent();
            BindingContext = new Extra();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            exDate.Date = DateTime.Today;            
        }

        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
            }

            (sender as Button).IsEnabled = true;
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var ext = (Extra)BindingContext;
            ext.Name = lugName.Text;
            ext.Description = description.Text;
            ext.Date = exDate.Date;
            ext.Hours = Convert.ToDouble(hours.Text);
            ext.Rate = Convert.ToDouble(rate.Text);

            ImageSource fg = image.Source;
            var byt = ImageSourceToBytes(fg);

            ext.Image.Add(byt);

            await App.Database.SaveExtraAsync(ext);
            await DisplayAlert("Alert", "Saved receipt", "OK");
            //// Navigate backwards
            //await Shell.Current.GoToAsync("..");
        }

        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            System.Threading.CancellationToken cancellationToken =
            System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            byte[] bytesAvailable = new byte[stream.Length];
            stream.Read(bytesAvailable, 0, bytesAvailable.Length);
            return bytesAvailable;
        }
    }
}