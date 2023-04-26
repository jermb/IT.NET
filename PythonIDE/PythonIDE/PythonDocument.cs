using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace PythonIDE
{
    internal class PythonDocument
    {
        private readonly MainPage main;
        private StorageFile file;
        private bool changed;

        //  Returns the name of the file minus the '.txt' extension
        public string FileName { get => file?.DisplayName; }
        public bool IsStored { get => file != null; }
        public bool HasChanges { get => changed; set { changed = value; main.UnsavedChanges(value); } }

        public PythonDocument(MainPage main)
        {
            this.main = main;
        }

        public async Task Save()
        {
            //  This method is called when saving an already existing file
            //  Uses the StorageFile saved in [file] to save
            await Save(file);
        }

        public async Task Save(StorageFile file)
        {
            if (file == null) return;
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteTextAsync(file, main.DocumentText);
            //  If there was an issue with saving the file, displays a message to the user
            Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            if (status != Windows.Storage.Provider.FileUpdateStatus.Complete) await new MessageDialog("An issue occurred while saving " + file.Name).ShowAsync();

            this.file = file;
            HasChanges = false;
            main.SetFileName();
        }

        public async Task<bool> Open(StorageFile file)
        {
            //  Returns true if file is successfully opened
            if (file == null) return false;   //  If a file is not chosen by the user
            try
            {
                main.DocumentText = await FileIO.ReadTextAsync(file);
                this.file = file;
                HasChanges = false;
                main.SetFileName();
                return true;
            }
            catch (Exception e) when (e is ArgumentException || e is FileNotFoundException || e is UnauthorizedAccessException || e is IOException || e is NotSupportedException || e is SecurityException)
            {
                await new MessageDialog("Could not open file.").ShowAsync();
            }
            return false;
        }

        public async Task New()
        {
            //  Defaults all values
            file = null;
            HasChanges = false;
            main.DocumentText = "";
            main.SetFileName();
        }
    }
}
