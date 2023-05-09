using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.UI.Xaml.Controls;

namespace PythonIDE
{
    class FolderNavigation : ObservableCollection<PythonItem>
    {
        public FolderNavigation() {}

        public void Set(PythonItem item)
        {
            //  Makes sure only one item is set to the tree at a time
            Clear();
            Add(item);
            //  Updates tree when items have relevant changes.
            item.PropertyChanged += OnPropertyChanged;
        }

        public void AddItem(PythonItem item)
        {
            if (item is PythonFile file && this[0] is PythonFolder folder)
            {
                folder.Add(file);
            }
            else
            {
                Set(item);
            }
        }

        public bool HasChanges()
        {
            //  Checks whether any file in file tree has unsaved changes
            foreach (var item in this)
            {
                if (item.HasChanges) return true;
            }
            return false;
        }

        public async Task Save()
        {
            //  Saves all files in file tree
            await this[0].Save();
        }

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            //  Refeshes tree
            Set(this[0]);
        }

        public PythonFile GetFirstFile()
        {
            //  Gets the first Python file in file tree
            if (this[0] is PythonFile file)
            {
                return file;
            }
            else if (this[0] is PythonFolder folder)
            {
                return folder.GetFirstFile();
            }
            //  if none exist returns a new file
            return new PythonFile();
        }

    }
}
