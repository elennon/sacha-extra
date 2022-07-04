using Extras.Models;
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
            var batches = await App.Database.GetBatchesAsync();
            collectionView.ItemsSource = batches;
            if (batches.Count == 0)
            {
                label1.Text = "Nothing sent yet...";
                label1.IsVisible = true;
            }            
        }
        private async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                var fd = collectionView.SelectedItem;
                Batch qt = (Batch)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(ViewBatchContent)}?{nameof(ViewBatchContent.ID)}={qt.BatchId}");
            }
        }
    }
}