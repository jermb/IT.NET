using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        ////private List<PythonFolder> subfolders;
        ////private List<PythonFile> files;

        //private ObservableCollection<PythonFile> files;

        //public event PropertyChangedEventHandler PropertyChanged;

        ////public List<PythonFolder> Subfolders { get => subfolders; set => subfolders = value; }
        ////public List<PythonFile> Files { get => files; set => files = value; }

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
            IReadOnlyList<IStorageItem> items = await thisFolder.GetItemsAsync();

            foreach (IStorageItem item in items)
            {
                if (item.IsOfType(StorageItemTypes.File) && ((StorageFile)item).FileType == ".py")
                {
                    PythonFile file = new PythonFile(item as StorageFile);
                    file.Open();
                    Add(file);
                }
                else if (item.IsOfType(StorageItemTypes.Folder))
                {
                    PythonFolder sub = new PythonFolder((StorageFolder)item);
                    sub.LoadItems();
                    Add(sub);
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

    }
}
