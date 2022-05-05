using Extras.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportingEPage : ContentPage
    {
        public ExportingEPage()
        {
            InitializeComponent();
            BindingContext = new ExportingExcelViewModel();
        }
    }
}