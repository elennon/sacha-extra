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
    public partial class CloseUp : ContentPage
    {
        public string ID
        {
            set
            {
                LoadExtra(value);
            }
        }
        public CloseUp()
        {
            InitializeComponent();
            BindingContext = new Extra();
        }
        async void LoadExtra(string ID)
        {
            try
            {
                int id = Convert.ToInt32(ID);
                Extra qt = await App.Database.GetExtraAsync(id);
                BindingContext = qt;
                siteName.Text = qt.JobSite;
                siteArea.Text = qt.SiteArea;
                exDate.Date = (DateTime)qt.Date;
                menNo.Text = qt.Men.ToString();
                description.Text = qt.Description;
                hours.Text = qt.Hours.ToString();
                rate.Text = qt.Rate.ToString();
                total.Text = qt.LaborCost.ToString();
                var hj = await App.Database.GetPicsAsync(qt.MyId);
                //var gy = (System.Collections.IEnumerable)App.Database.GetPicsAsync(qt.ID);
                // var fttt = hj.Select(x => x.Pic).ToList();
                var df = hj.Select(x => x.FileName).ToList();
                ImgCarouselView.ItemsSource = df;
                
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load quote.");
            }
        }
    }
}