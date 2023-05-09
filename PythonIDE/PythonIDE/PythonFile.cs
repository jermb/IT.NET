using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace PythonIDE
{
    internal class PythonFile : PythonItem
    {
        private StorageFile file;
        private bool changed;

        public override Symbol Symbol { get => Symbol.Document; }

        //  Returns the name of the file minus the '.txt' extension
        public string FileName { get => file?.DisplayName; }
        public bool IsStored { get => file != null; }
        public override bool HasChanges { get => changed; 
            set 
            {
                if (changed != value)
                {
                    changed = value;
                    OnPropertyChanged("HasChanges");
                    MainPage.UnsavedChanges(value);
                }
            } 
        }

        public override string DisplayName { get => (changed) ? "*" + name : name; }

        public string Contents { get => contents; set => contents = value; }
        private string contents;

        public PythonFile()
        {
            name = "Untitled";
        }

        public PythonFile(StorageFile file)
        {
            this.file = file;
            name = file?.DisplayName;
        }

        public override async Task Save()
        {
            contents = MainPage.GetContent();
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteTextAsync(file, contents);
            //  If there was an issue with saving the file, displays a message to the user
            Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            if (status != Windows.Storage.Provider.FileUpdateStatus.Complete) await new MessageDialog("An issue occurred while saving " + file.Name).ShowAsync();
            HasChanges = false;
        }

        public async Task Save(StorageFile file)
        {
            if (file == null) return;
            this.file = file;
            name = file.DisplayName;
            OnPropertyChanged("DisplayName");
            await Save();
        }

        public async Task<(string, string)> Open()
        {
            //  Returns true if file is successfully opened
            if (file == null) return (null, null);   //  If a file is not chosen by the user
            try
            {
                contents = await FileIO.ReadTextAsync(file);
                HasChanges = false;
                name = file.DisplayName;
                return (FileName, contents);
            }
            catch (Exception e) when (e is ArgumentException || e is FileNotFoundException || e is UnauthorizedAccessException || e is IOException || e is NotSupportedException || e is SecurityException)
            {
                await new MessageDialog("Could not open file.").ShowAsync();
            }
            return (null, null);
        }

        //  <Name, Contents>
        public async Task<(string, string)> Load()
        {
            if (file == null) return (null, null);

            if (contents != null && name != null) return (name, contents);

            return await Open();
        }

        public void Run()
        {
            //var stream = await file.OpenReadAsync();
            //var pythonCode = new StreamReader(stream.AsStream()).ReadToEnd();

            Debug.WriteLine(contents);

            var engine = Python.CreateEngine();
            var eio = engine.Runtime.IO;
            var errors = new MemoryStream();
            eio.SetErrorOutput(errors, Encoding.Default);

            var results = new MemoryStream();
            eio.SetOutput(results, Encoding.Default);

            var scope = engine.CreateScope();
            engine.Execute(contents, scope);

            string str(byte[] x) => Encoding.Default.GetString(x);



            MainPage.SetOutput(str(results.ToArray()), str(errors.ToArray()));
        }

    }
}
