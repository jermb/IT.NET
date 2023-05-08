using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //public FolderNavigation
        public FolderNavigation()
        {

        }

        public void Set(PythonItem item)
        {
            Clear();
            Add(item);
        }

        public async Task Save()
        {
            await this[0].Save();
        }

        //public override string DisplayName { get => name; }
        //private string name;



        ////public void Set(PythonFolder folder)
        ////{
        ////    Clear();
        ////    Add(folder);
        ////}

        //public void Add(string name)
        //{
        //    Items.Add(new FolderNavigation(name));
        //}

    }
    class Item
    {
        public string Name { get; set; }
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();
        public Item(string name)
        {
            Name = name;
            Items.Add(new Item());
            Items.Add(new Item());
        }

        public Item()
        {
            Name = "sub";
        }
    }
}
