using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PythonIDE
{
    internal class PythonFolder : PythonItem
    {

        private readonly StorageFolder thisFolder;

        public override bool HasChanges { 
            get 
            {
                foreach (var item in Items)
                {
                    if (item.HasChanges) return true;
                }
                return false;
            }
            set { }
        }

        //  Icon displayed along name;
        public override Symbol Symbol { get => Symbol.Folder; }

        public PythonFolder()
        {
            name = "no folder";
        }

        public PythonFolder(StorageFolder thisFolder)
        {
            this.thisFolder = thisFolder;
            name = thisFolder.DisplayName;
        }

        public async void LoadItems()
        {
            //  Gets items in folder and properly casts them to either PythonFolder or PythonFile
            IReadOnlyList<IStorageItem> items = await thisFolder.GetItemsAsync();

            foreach (IStorageItem item in items)
            {
                if (item.IsOfType(StorageItemTypes.File) && ((StorageFile)item).FileType == ".py")
                {
                    PythonFile file = new PythonFile(item as StorageFile);
                    file.Open();
                    Add(file);
                    file.PropertyChanged += OnPropertyChanged;
                }
                else if (item.IsOfType(StorageItemTypes.Folder))
                {
                    PythonFolder sub = new PythonFolder((StorageFolder)item);
                    sub.LoadItems();
                    Add(sub);
                    sub.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        public void Add(PythonItem item)
        {
            Items.Add(item);
        }

        public override async Task Save()
        {
            foreach (PythonItem item in Items)
            {
                await item.Save();
            }
        }

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        public PythonFile GetFirstFile()
        {
            foreach (var item in Items)
            {
                if (item is PythonFile file)
                {
                    return file;
                }
                else if (item is PythonFolder folder)
                {
                    return folder.GetFirstFile();
                }
            }
            return new PythonFile();
        }

    }
}
