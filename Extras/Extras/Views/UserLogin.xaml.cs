using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extras.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserLogin : ContentPage
    {
        public static Label label { get; set; }
        public static Entry passWd { get; set; }
        public UserLogin()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = new UserLoginPageViewModel(Navigation);
            label = label1;
            passWd = passWd1;
        }
    }
}