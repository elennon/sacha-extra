using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
    public partial class VeiwAll : ContentPage
    {
        public VeiwAll()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var h = await App.Database.GetExtrasAsync();
            collectionView.ItemsSource = h;
            pic.Source = ImageSource.FromStream(() => new MemoryStream(h.FirstOrDefault().Image));

        }
        
    }

    
}