using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace TextEditor
{
    public class TextDocument
    {
        private MainPage main;
        private TextBox textbox;
        private StorageFile file;
        private bool changed;
        private string text;

        public string FilePath { get => file?.Path; }
        public string FileName { get => file?.Name; }
        public StorageFile File { get => file; }
        public bool HasChanges { get => changed; set { changed = value; main.SaveIsEnabled(value); } }

        public string Text { get => text; }

        public TextDocument(TextBox textbox, MainPage main)
        {
            this.textbox = textbox;
            this.main = main;
        }

        public async void Save()
        {
            Save(File);
        }

        public async void Save(StorageFile file)
        {
            if (file == null) return;
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteTextAsync(file, textbox.Text);
            Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

            HasChanges = false;

            if (status != Windows.Storage.Provider.FileUpdateStatus.Complete) await new MessageDialog("An issue occurred while saving " + file.Name).ShowAsync();
        }

        public async void Open(StorageFile file)
        {
            if (file == null) await new MessageDialog("Could not open file.").ShowAsync();

            textbox.Text = text = await FileIO.ReadTextAsync(file);
            this.file = file;
            HasChanges = false;
        }

        public void New()
        {
            file = null;
            HasChanges = false;
            textbox.Text = text = "";
        }

    }

}
