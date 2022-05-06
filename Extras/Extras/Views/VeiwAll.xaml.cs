using Xamarin.Essentials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Extras.ViewModels;
using Extras.Services;
using Extras.Models;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VeiwAll : ContentPage
    {
        private ExcelService excelService;
        public VeiwAll()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var h = await App.Database.GetExtrasAsync();
            collectionView.ItemsSource = h;
            excelService = new ExcelService();
        }
        async void OnBackupButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "backup.json");
                var alal = await App.Database.GetExtrasAsync();
                string json1 = JsonConvert.SerializeObject(alal);
                File.WriteAllText(fileName, json1);
                
                List<string> toAddress = new List<string>();
                toAddress.Add("elennon@outlook.ie");
                await SendEmail("this a sublect", "tuna bod", toAddress, fileName);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Exception: " + ex.InnerException.ToString(), "OK");
            }
        }
        
        public async Task SendEmail(string subject, string body, List<string> recipients, string filename)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                message.Attachments.Add(new EmailAttachment(filename));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await DisplayAlert("Alert", "Exception: " + fbsEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Exception: " + ex.Message, "OK");
            }
        }

        private async void sendAsEmailClicked(object sender, EventArgs e)
        {
            var dg = collectionView.SelectedItems;
            List<Extra> h = new List<Extra>();
            foreach (var item in dg)
            {
                h.Add((Extra)item);
            }
            var exfile = ExportToExcel(h);
            List<string> toAddress = new List<string>();
            toAddress.Add(emailto.Text);
            await SendEmail("this a sublect", "tuna bod", toAddress, exfile);
        }
        private string ExportToExcel(List<Extra> sleected)
        {
            var fileName = $"Extras-{Guid.NewGuid()}.xlsx";
            string filepath = excelService.GenerateExcel(fileName);

            var data = new ExcelStructure
            {
                Headers = new List<string>() { "Job Site", "Employee", "Description", "Hours", "Rate", "LaborCost", "Date" }
            };

            foreach (var item in sleected)
            {
                data.Values.Add(new List<string>() {item.JobSite, item.Name, item.Description, item.Hours.ToString(), 
                    item.Rate.ToString(), item.LaborCost.ToString(), item.Date.ToString() });
            }

            excelService.InsertDataIntoSheet(filepath, "Contacts", data);

            return filepath;
        }

    }
}