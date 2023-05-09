using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PythonIDE
{
    internal abstract class PythonItem : INotifyPropertyChanged
    {
        public virtual string DisplayName { get => name; }
        public virtual bool HasChanges { get; set; }

        protected string name;


        public abstract Symbol Symbol { get; }

        public virtual ObservableCollection<PythonItem> Items { get; } = new ObservableCollection<PythonItem>();

        public virtual async Task Save() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
