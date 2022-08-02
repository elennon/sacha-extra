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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VeiwAll : ContentPage
    {
        private List<Extra> extrs = new List<Extra>();
        private Gemmers gemmer;
        private Batch batch = new Batch();
        private Project currentProject = new Project();
        ObservableCollection<Extra> myCollection;
        private string AppFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        public VeiwAll()
        {
            InitializeComponent();
            BindingContext = new Extra();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            currentProject = await App.Database.GetCurrentProjectAsync();
            if(currentProject != null)
            {
                extrs = await App.Database.GetExtrasAsync(currentProject.MyId);
                extrs = extrs.Where(x => x.WasSent == false).ToList();
                myCollection = new ObservableCollection<Extra>(extrs);
                collectionView.ItemsSource = myCollection;
                gemmer = new Gemmers();
                if (extrs.Count == 0)
                {
                    emptyLabel.Text = "No extras to send...";
                    emptyLabel.IsVisible = true;
                }
                else
                {
                    emptyLabel.Text = "No extras to send...";
                    emptyLabel.IsVisible = false;
                }
                var sub = GetPw("EmailSubject").Result;
                if (sub != null)
                {
                    subject.Text = sub;
                }
                var ebody = GetPw("EmailBody").Result;
                if (ebody != null)
                {
                    body.Text = ebody;
                }
            }
            else
            {
                await DisplayAlert("Alert", "There is no project selected as current project. Please add a project and set it as current project.", "OK");
                await Shell.Current.GoToAsync(nameof(ProjectsPage));
            }            
        }
        private async Task<string> GetPw(string key)
        {
            string oauthToken = null;
            try
            {
                oauthToken = await SecureStorage.GetAsync(key);
            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }
            return oauthToken;
        }
        private async Task SetPw(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
        async void OnBackupButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "backup.json");
                var alal = await App.Database.GetExtrasAsync(App.Database.GetCurrentProjectAsync().Result.MyId);
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
                Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                message.Attachments.Add(new EmailAttachment(filename));
                var zipFile = Path.Combine(AppFolder, @"Dayworks-images.zip");
                if (File.Exists(zipFile)) { File.Delete(zipFile); }
                var hasPics = false;
                using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                {
                    foreach (var earia in extrs.GroupBy(x => x.SiteArea))
                    {
                        foreach (var item in earia)
                        {
                            var pics = await App.Database.GetPicsAsync(item.MyId);
                            if (pics.Count != 0)
                            {
                                hasPics = true;
                                var count = 1;
                                foreach (var pc in pics)
                                {
                                    string nme = "";
                                    if (Device.RuntimePlatform == Device.iOS)
                                    {
                                        nme = "Excel Ref Num " + item.ID + " - " + count + ".jpg";
                                    } else
                                    {
                                        var ext = pc.FileName.Split('.');
                                        nme = "Excel Ref Num " + item.ID + " - " + count + "." + ext[ext.Length - 1];                                       
                                    }
                                    archive.CreateEntryFromFile(pc.FileName, nme);
                                    count++;
                                }
                            }
                        }
                    }
                }               
                if(hasPics) message.Attachments.Add(new EmailAttachment(zipFile));
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
            if (subject.Text == null || emailto.Text == null || body.Text == null || subject.Text == "" || emailto.Text == "" || body.Text == "")
            {
                await DisplayAlert("Not Sent", "Please fill in email address, subject and body", "OK");
                return;
            }
            
            if(!ValidateEmail(emailto.Text))
            {
                await DisplayAlert("", "Please enter a valid email address", "OK");
                return;
            }
            SetPw("EmailSubject", subject.Text).Wait();
            SetPw("EmailBody", body.Text).Wait();
            if (extrs.Count != 0)
            {
                batch = new Batch();
                batch.ProjectName = currentProject.ProjectName;
                batch.DateSent = DateTime.Now;
                batch.BatchId = Guid.NewGuid().ToString();
                var b = App.Database.SaveBatchAsync(batch);
                
                var exfile = gemmer.GetGemmer(extrs);
                List<string> toAddress = new List<string>();
                toAddress.Add(emailto.Text);
                await SendEmail(subject.Text, body.Text, toAddress, exfile, extrs);
                File.Delete(exfile);               
                extrs.ForEach(x => x.BatchId = batch.BatchId);
                extrs.ForEach(x => x.WasSent = true);
                extrs.ForEach(x => App.Database.SaveExtraAsync(x));
            }
            else
            {
                await DisplayAlert("Not Sent", "Please add some extras before sending", "OK");
                return;
            }
            
        }
        async void OnImageNameTapped(object sender, EventArgs e)
        {
            try
            {
                var result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync("Are you sure you want to delete this Extra?", "Confirm Delete", "Yes", "No");
                if (result)
                {
                    Extra ext = (Extra)(sender as Image).BindingContext;
                    if (ext != null)
                    {
                        await App.Database.DeleteExtraAsync(ext);
                        myCollection.Remove(ext);                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
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