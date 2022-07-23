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
                //int id = Convert.ToInt32(ID);
                Extra qt = await App.Database.GetExtraAsync(ID);
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
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var ext = (Extra)BindingContext;
                if (menNo.Text == null || menNo.Text == "")
                {
                    await DisplayAlert("Not Saved", "You need to add number of men", "OK");
                    return;
                }
                ext.Men = Convert.ToInt16(menNo.Text);
                if (description.Text == null || description.Text == "")
                {
                    await DisplayAlert("Not Saved", "You need to add a description", "OK");
                    return;
                }
                ext.Description = description.Text;
                ext.Date = exDate.Date;
                if (hours.Text == null || hours.Text == "")
                {
                    await DisplayAlert("Not Saved", "You need to add number of hours", "OK");
                    return;
                }
                ext.Hours = Convert.ToDouble(hours.Text);
                if (rate.Text == null || rate.Text == "")
                {
                    ext.Rate = 0;
                }
                ext.Rate = Convert.ToDouble(rate.Text);
                ext.JobSite = siteName.Text;
                if (siteArea.Text == null)
                {
                    await DisplayAlert("Not Saved", "You need to add the site area", "OK");
                    return;
                }
                ext.SiteArea = siteArea.Text;                
                var iid = App.Database.SaveExtraAsync(ext);
                await DisplayAlert("Changes Saved", "", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Alert", "Exception: " + e.ToString(), "OK");
            }
        }
    }
}