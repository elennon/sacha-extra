using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.ObjectModel;

namespace Extras.Models
{
    public class Project : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MyId { get; set; }
        //public string ProjectName { get; set; }
        private String _Text;
        public String ProjectName
        {
            get => _Text;
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Address { get; set; }

        private Boolean _IsCurrent;
        public Boolean IsCurrent
        {
            get => _IsCurrent;
            set
            {
                if (_IsCurrent != value)
                {
                    _IsCurrent = value;
                    OnPropertyChanged();
                }
            }
        }

        private Boolean _Checked;
        public Boolean IsChecked
        {
            get => _Checked;
            set
            {
                if (_Checked != value)
                {
                    _Checked = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Projects : ObservableCollection<Project>, INotifyPropertyChanged
    {
        public Projects(List<Project> prjs)
        {
            foreach (var item in prjs)
            {
                this.Add(item);
            }
        }
        public Projects() { }

        private Project _CostumerSelected;
        public Project CostumerSelected
        {
            get => _CostumerSelected;
            set
            {
                if (value != null)
                {
                    if (_CostumerSelected != value)
                    {
                        if (_CostumerSelected != null) _CostumerSelected.IsChecked = false;
                        _CostumerSelected = value;
                        OnPropertyChanged();
                        _CostumerSelected.IsChecked = true;
                    }
                }               
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                _CostumerSelected.IsCurrent = true;
                _CostumerSelected.IsChecked = true;
                App.Database.SaveProjectAsync(_CostumerSelected);
                var all = await App.Database.GetProjectsAsync();
                all.Where(x => x.MyId != _CostumerSelected.MyId).ToList().ForEach(x => setToUnChecked(x));
                this.Where(x => x.MyId != _CostumerSelected.MyId).ToList().ForEach(x => setToUnChecked(x));
                var aill = await App.Database.GetProjectsAsync();
                var sd = aill.Count;
            }
        }
        private void setToUnChecked(Project x)
        {
            x.IsChecked = false;
            x.IsCurrent = false;
            App.Database.SaveProjectAsync(x);
        }
    }
}
