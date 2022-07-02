using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviousBatches : ContentPage
    {
        public PreviousBatches()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();           
            collectionView.ItemsSource = await App.Database.GetBatchesAsync();
        }
        private void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}