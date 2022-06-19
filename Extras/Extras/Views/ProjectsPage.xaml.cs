using Extras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPage
    {
        //private List<Project> projects = new List<Project>();
        Projects projs;// = new Projects();
        public ProjectsPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var prjs = await App.Database.GetProjectsAsync();
            projs = new Projects(prjs);
            prjsView.ItemsSource = projs;
        }


        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var prj = new Project();
                prj.MyId = Guid.NewGuid().ToString();
                prj.ProjectName = siteName.Text;
                prj.Address = siteAddress.Text;
                prj.IsCurrent = ckbCurrent.IsChecked;

                var iid = App.Database.SaveProjectAsync(prj);
                var dc = new List<Project>() { prj };
                projs.Add(prj);

                await DisplayAlert("Saved", "", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Alert", "Exception: " + e.ToString(), "OK");
            }
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Application.Current.Properties["IsLoggedIn"] = Boolean.FalseString;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}