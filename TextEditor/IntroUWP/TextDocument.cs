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
        private TextBox textbox;

        public string FilePath { get; set; }
        public string FileName { get; set; }

        public StorageFile File { get; }
        public bool HasChanges { get; set; }

        public string Text { get; set; }

        public TextDocument(TextBox textbox)
        {
            this.textbox = textbox;
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
            string text = await FileIO.ReadTextAsync(file);
            FilePath = file.Path;
            FileName = file.Name;
            HasChanges = false;
            Text = text;
            textbox.Text = text + "\n" + FilePath;
        }

        public void New()
        {
            FilePath = FileName = File = null;
            HasChanges = false;
            textbox.Text = Text = "";
        }

        public async Task<StorageFolder> FileDirectory()
        {
            return await StorageFolder.GetFolderFromPathAsync(FilePath);
        }

    }

}
