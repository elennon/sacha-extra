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
using System.IO.Compression;
using System.Reflection;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VeiwAll : ContentPage
    {
        //private ClosedExcelService excelService;
        private List<Extra> extrs = new List<Extra>();
        private Gemmers gemmer;
        private string AppFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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
            //excelService = new ClosedExcelService();
            gemmer = new Gemmers();
            
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
                FileInfo fi = new FileInfo(filename);
                var allBytes = fi.Length;
                Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

                var zippers = new List<KeyValuePair<string, List<string>>>();
                var cnt = 1; bool onlyOne = true; List<string> list = new List<string>();
                foreach (var earia in extrs.GroupBy(x => x.SiteArea))
                {
                    foreach (var item in earia)
                    {
                        var pics = await App.Database.GetPicsAsync(item.MyId);
                        if (pics != null)
                        {                           
                            foreach (var fPath in pics)
                            {
                                fi = new FileInfo(fPath.FileName);
                                allBytes += fi.Length;
                                if (allBytes <= 24000000)
                                {
                                    list.Add(fPath.FileName);
                                }
                                else
                                {
                                    onlyOne = false;
                                    zippers.Add(new KeyValuePair<string, List<string>>("zipFile-" + cnt, list));
                                    cnt++;
                                    list.Clear();
                                }
                            }
                            
                        }
                    }
                }
                if (onlyOne) zippers.Add(new KeyValuePair<string, List<string>>("zipFile-" + cnt, list));

                foreach (var item in zippers)
                {
                    var message = new EmailMessage
                    {
                        Subject = subject,
                        Body = body,
                        To = recipients,
                    };
                    message.Attachments.Add(new EmailAttachment(filename));
                    
                    var zipFile = Path.Combine(AppFolder, item.Key + @"-Dayworks-images.zip");
                    if (File.Exists(zipFile)) { File.Delete(zipFile); }
                    using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                    {
                        foreach (var fle in item.Value)
                        {
                            archive.CreateEntryFromFile(fle, "img - " + Path.GetFileName(fle));
                        }
                    }
                    message.Attachments.Add(new EmailAttachment(zipFile));
                    await Email.ComposeAsync(message);
                }                
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

            //var exfile = await excelService.Export(extrs);
            if (extrs.Count != 0)
            {
                var exfile = gemmer.GetGemmer(extrs);
                List<string> toAddress = new List<string>();
                toAddress.Add(emailto.Text);
                await SendEmail(subject.Text, body.Text, toAddress, exfile, extrs);
            }
            else
            {
                await DisplayAlert("Alert", "Please add some extras before sending", "OK");
            }
            
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