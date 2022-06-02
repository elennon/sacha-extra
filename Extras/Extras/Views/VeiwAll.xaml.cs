using Xamarin.Essentials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Extras.Services;
using Extras.Models;
using System.Linq;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VeiwAll : ContentPage
    {
        private ClosedExcelService excelService;
        private List<Extra> extrs = new List<Extra>();
        public VeiwAll()
        {
            InitializeComponent();
            BindingContext = new Extra();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            extrs = await App.Database.GetExtrasAsync();
            collectionView.ItemsSource = extrs;
            excelService = new ClosedExcelService();
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
                await SendEmail("Extras excel attached", "please find attached a copy of the excel file", toAddress, fileName, alal);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Exception: " + ex.InnerException.ToString(), "OK");
            }
        }
        
        public async Task SendEmail(string subject, string body, List<string> recipients, string filename, List<Extra> extrs)
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
                //foreach (var item in extrs)
                //{
                //    if (item.)
                //    {

                //    }
                //}

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
           
            var exfile = await excelService.Export(extrs);
            List<string> toAddress = new List<string>();
            toAddress.Add(emailto.Text);
            await SendEmail(subject.Text, body.Text, toAddress, exfile, extrs);
        }

        private async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the NoteEntryPage, passing the ID as a query parameter.
                Extra qt = (Extra)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(CloseUp)}?{nameof(CloseUp.ID)}={qt.ID.ToString()}");
                //await Shell.Current.GoToAsync($"{nameof(UpdatePage)}?{nameof(UpdatePage.ID)}={qt.ID.ToString()}");
            }
        }
    }
}