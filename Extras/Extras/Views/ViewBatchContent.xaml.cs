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
    [QueryProperty(nameof(ID), nameof(ID))]
    public partial class ViewBatchContent : ContentPage
    {
        private List<Extra> extrs = new List<Extra>();
        Batch bt = new Batch();
        public string ID
        {
            set
            {
                LoadBatch(value);
            }
        }
        public ViewBatchContent()
        {
            InitializeComponent();           
        }
        async void LoadBatch(string ID)
        {
            try
            {                
                bt = await App.Database.GetBatchAsync(ID);
                extrs = await App.Database.GetBatchExtrasAsync(bt);
                collectionView.ItemsSource = extrs;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load Batch.");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                var fd = collectionView.SelectedItem;

                // Navigate to the NoteEntryPage, passing the ID as a query parameter.
                Extra qt = (Extra)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(CloseUp)}?{nameof(CloseUp.ID)}={qt.MyId}");
            }
        }
    }
}