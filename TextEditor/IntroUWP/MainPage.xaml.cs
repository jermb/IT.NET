using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Text;
using Windows.ApplicationModel.Core;
using System.Reflection;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TextEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        TextDocument doc; 
        public MainPage()
        {
            this.InitializeComponent();
            doc = new TextDocument(InputBox, this);
        }

        private async void Open(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            doc.Open(file);
        }

        private async void Save(object sender, RoutedEventArgs e)
        {
            if (doc.File == null) { SaveAs(sender, e); return; }
            doc.Save();
        }

        private async void SaveAs(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            doc.Save(file);
        }

        private async void New(object sender, RoutedEventArgs e)
        {
            if (!doc.HasChanges || await ResolveUnsavedChanges(sender, e)) doc.New();
        }

        private async void Exit(object sender, RoutedEventArgs e)
        {
            //  Checks whether document has unsaved changes, if so displays dialog to allow user to save or cancel operation.
            //  If user saves or discards changes the application will close. 
            //  Otherwise the close operation is canceled
            if (!doc.HasChanges || await ResolveUnsavedChanges(sender, e)) CoreApplication.Exit();
        }

        private async void About(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("This application is a basic text editor. It is capable of opening and saving '.txt' files.\nIt was created by Jay Edson for INFOTC 4400").ShowAsync();
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            doc.HasChanges = true;
        }

        private async Task<bool> ResolveUnsavedChanges(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("You have unsaved changes.", "Exit?");
            var saveCommand = new UICommand("Save");
            var discardCommand = new UICommand("Discard");
            var cancelCommand = new UICommand("Cancel");
            dialog.Commands.Add(saveCommand);
            dialog.Commands.Add(discardCommand);
            dialog.Commands.Add(cancelCommand);

            var command = await dialog.ShowAsync();
            if (command == cancelCommand)
            {
                return false;
            }
            else if (command == saveCommand)
            {
                Save(sender, e);
            }
            return true;
        }

        private void TabHandling(object sender, KeyRoutedEventArgs e)
        {
            //if (e.Key == Windows.System.VirtualKey.Tab)
            //{
            //    int cursor = InputBox.SelectionStart;
            //    InputBox.Text = InputBox.Text.Insert(cursor, "\t");
            //    e.Handled = true;
            //}
        }

        public void SaveIsEnabled(bool enabled)
        {
            SaveButton.IsEnabled = enabled;
        }
    }
}
