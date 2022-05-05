using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Extras.Models;
using Extras.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Extras.ViewModels
{
    public class ExportingExcelViewModel
    {
        public  ICommand ExportToExcelCommand { private set; get; }
        private ExcelService excelService;
        public  ObservableCollection<Extra> contacts;
        

        public ExportingExcelViewModel()
        {
            contacts = new ObservableCollection<Extra>
            {
                new Extra{  Name="Leomaris", Description="Reyes",   Hours=80 },
                new Extra{  Name="Leo",      Description="Rosario", Hours=10 },
                new Extra{  Name="Mary",     Description="Mendez",  Hours=30 }, 
            };

            ExportToExcelCommand = new Command(async () => await ExportToExcel());
            excelService = new ExcelService();
        }

        async Task ExportToExcel()
        { 
            var fileName = $"Contacts-{Guid.NewGuid()}.xlsx";
            string filepath = excelService.GenerateExcel(fileName);

            var data = new ExcelStructure
            {
                Headers = new List<string>() { "Name", "Description", "Hours" }
            };

            foreach (var item in contacts)
            { 
                data.Values.Add(new List<string>() { item.Name, item.Description, item.Hours.ToString() });
            }

            excelService.InsertDataIntoSheet(filepath, "Contacts",data);

            await Launcher.OpenAsync(new OpenFileRequest()
            {
                File = new ReadOnlyFile(filepath)
            });
        }

        
    }
}
