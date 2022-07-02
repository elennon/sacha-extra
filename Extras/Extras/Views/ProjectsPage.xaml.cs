using Extras.Models;
using System;
using System.Collections.Generic;
using Extras.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Extras.Helpers;

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
            //BindingContext = new DetailsPageViewModel(Navigation);
            BindingContext = new Projects();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var prjs = await App.Database.GetProjectsAsync();
            projs = new Projects(prjs);
            BindingContext = projs;
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
                prj.IsChecked = ckbCurrent.IsChecked;           
                
                var prjs = await App.Database.GetProjectsAsync();
                if (ckbCurrent.IsChecked)
                {
                    prjs.ForEach(x => setToUnChecked(x));
                }
                var iid = App.Database.SaveProjectAsync(prj);
                projs.Clear();
                prjs.ForEach(x => projs.Add(x));
                projs.Add(prj);
                await DisplayAlert("Saved", "", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Alert", "Exception: " + e.ToString(), "OK");
            }
        }
        private void setToUnChecked(Project x)
        {
            x.IsChecked = false;
            x.IsCurrent = false;
            App.Database.SaveProjectAsync(x);
        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            //var f = new DetailsPageViewModel(Navigation);
            if (UserLogin.passWd != null)
            {
                UserLogin.passWd.Text = "";
            }
            Navigation.PushAsync(new UserLogin());
            UserSettings.ClearAllData();
        }
    }
}