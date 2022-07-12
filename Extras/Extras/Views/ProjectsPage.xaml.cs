using Extras.Models;
using System;
using System.Collections.Generic;
using Extras.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Extras.Helpers;
using Acr.UserDialogs;

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
        //protected override async void OnDisappearing()
        //{
        //    var currentProject = await App.Database.GetCurrentProjectAsync();
        //    if (currentProject == null)
        //    {
        //        await DisplayAlert("Alert", "There is no project selected as current project. Please add a project and set it as current project.", "OK");
        //    }
        //}
        async void OnImageNameTapped(object sender, EventArgs e)
        {
            try
            {
                var result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync("Are you sure you want to delete this Project?", "Confirm Delete", "Yes", "No");
                if (result)
                {
                    Project prj = (Project)(sender as Image).BindingContext;
                    if (prj != null)
                    {                        
                        await App.Database.DeleteProjectAsync(prj);
                        projs.Remove(prj);  
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (siteName.Text == null)
            {
                await DisplayAlert("Not Saved", "You need to add a Project name", "OK");
                return;
            }
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
                if (projs.Count == 0)
                {
                    prj.IsCurrent = true;
                    prj.IsChecked = true;
                }
                var iid = App.Database.SaveProjectAsync(prj);
                projs.Clear();
                prjs.ForEach(x => projs.Add(x));

                projs.Add(prj);
                Acr.UserDialogs.UserDialogs.Instance.Toast("Project saved!");
            }
            catch (Exception)
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast("Exception:" + e.ToString());
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