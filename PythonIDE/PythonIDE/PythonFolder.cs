using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace PythonIDE
{
    internal class PythonFolder : INotifyPropertyChanged
    {

        private readonly StorageFolder thisFolder;
        private List<PythonFolder> subfolders;
        private List<PythonFile> files;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<PythonFolder> Subfolders { get => subfolders; set => subfolders = value; }
        public List<PythonFile> Files { get => files; set => files = value; }

        public PythonFolder(StorageFolder thisFolder)
        {
            this.thisFolder = thisFolder;
        }

        public async void LoadItems()
        {
            IReadOnlyList<IStorageItem> items = await thisFolder.GetItemsAsync();

            foreach (IStorageItem item in items)
            {
                if (item.IsOfType(StorageItemTypes.File) && ((StorageFile) item).FileType == ".py")
                {
                    files.Add(new PythonFile((StorageFile) item));
                }
                else if (item.IsOfType(StorageItemTypes.Folder))
                {
                    PythonFolder sub = new PythonFolder((StorageFolder) item);
                    sub.LoadItems();
                    subfolders.Add(sub);
                }
            }
        }

        protected void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropertyChanged)));
        }

    }
}
