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


namespace TextEditor
{
    public sealed partial class MainPage : Page
    {

        private readonly TextDocument doc; 
        public MainPage()
        {
            this.InitializeComponent();
            doc = new TextDocument(this);
        }

        /* Event Handlers */
        private async void Open(object sender, RoutedEventArgs e)
        {
            //  Make sure the user doesn't unwillingly delete changes
            if (!await ResolveUnsavedChanges("Are you sure?")) return;

            //  Opens a dialog to choose a file
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            //  Sends a reference to the chosen file to the TextDocument class to be read
            //  If a new document is successfully opened ensures the New document button is available
            if (await doc.Open(file)) NewButton.IsEnabled = true;
        }

        private async void Save(object sender, RoutedEventArgs e)
        {
            //  The Save() methods is split into two parts: the Save(sender, e) event handler and Save() the actual functional Task
            //  This is becuase Event Handlers must return void, but when saving using the ResolveUnsavedChanges() dialog they need to be awaited
            //  Otherwise a new document can be created before the current document is fully saved which can cause issues.
            await Save();
        }
        private async Task Save()
        {
            //  If the file has not been saved before redirect to the Save As function so the user can name it and choose it's location.
            if (!doc.IsStored) { await SaveAs(); return; }
            //  If it has a name and location, save it quietly
            await doc.Save();
        }

        private async void SaveAs(object sender, RoutedEventArgs e)
        {
            //  See Save(sender, e)
            await SaveAs();
        }

        private async Task SaveAs()
        {
            //  Opens a dialog for the user to name and pick the location of their file
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            //  If the Document already has a name fill that into the file name field, otherwise default to 'Untitled'
            savePicker.SuggestedFileName = doc.FileName ?? "Untitled";
            StorageFile file = await savePicker.PickSaveFileAsync();
            //  Sends the file to the TextDocument class to be written to
            await doc.Save(file);
        }

        private async void New(object sender, RoutedEventArgs e)
        {
            //  If the current doc has Unsaved changes ask the user to save them before opening a new document
            //  If they cancel the operation nothing happens, otherwise a new document is created.
            if (await ResolveUnsavedChanges("Are you sure?")) await doc.New();
            NewButton.IsEnabled = false;
        }

        private async void Exit(object sender, RoutedEventArgs e)
        {
            //  Checks whether document has unsaved changes, if so displays dialog to allow user to save or cancel operation.
            //  If user saves or discards changes the application will close. 
            //  Otherwise the close operation is canceled
            if (await ResolveUnsavedChanges("Exit?")) CoreApplication.Exit();
        }

        private async void About(object sender, RoutedEventArgs e)
        {
            //  Displays a Dialog box with information about the application
            await new MessageDialog("This application is a basic text editor. It is capable of opening and saving '.txt' files.\nIt was created by Jay Edson for INFOTC 4400", "About").ShowAsync();
        }

        /*  Utility Methods  */
        private async Task<bool> ResolveUnsavedChanges(string msgTitle)
        {
            if (!doc.HasChanges) return true;
            //  Checks whether the user really wants to exit the application/current document with unsaved changes
            var dialog = new MessageDialog("You have unsaved changes.", msgTitle);
            //  Creates buttons and then adds them to the dialog box
            var saveCommand = new UICommand("Save");
            var discardCommand = new UICommand("Discard");
            var cancelCommand = new UICommand("Cancel");
            dialog.Commands.Add(saveCommand);
            dialog.Commands.Add(discardCommand);
            dialog.Commands.Add(cancelCommand);

            var command = await dialog.ShowAsync();
            if (command == cancelCommand)
            {
                //  Cancelling returns false and prevents whatever action was occurring
                return false;
            }
            else if (command == saveCommand)
            {
                await Save();
            }
            //  Saving or discarding changes continues operation
            return true;
        }

        private void TabHandling(object sender, KeyRoutedEventArgs e)
        {
            //  This method is technically a KeyDown event, so any time a user types we use that to tell the application the document has been changed
            doc.HasChanges = true;
            //  Watch for a Tab press and handle as input rather than moving to next navigation item
            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                //  Finds the area selected by the user and replaces it with a Tab character
                //  Allows for replacing multiple characters with one tab, as opposed to just inserting a tab where the cursor is
                int start = InputBox.SelectionStart;
                int length = InputBox.SelectionLength;
                InputBox.Text = InputBox.Text.Substring(0, start) + "\t" + InputBox.Text.Substring(start + length);
                //  Moves the cursor behind the new tab
                InputBox.SelectionStart = ++start;
                //  Absorbs tab press, prevents moving to next navigation item
                e.Handled = true;
            }
        }

        /*  Public methods called from TextDocument class  */
        public string DocumentText 
        { 
            get => InputBox.Text;
            //  Prevents setting the TextBox to hold a null value
            set => InputBox.Text = value ?? ""; 
        }
        public void SetFileName()
        {
            //  Sets the Title of the document about the text box to the file's name (if it has one), defaults to 'Untitled' (same as SaveAs())
            FileName.Text = doc.FileName ?? "Untitled";
        }

        public void UnsavedChanges(bool enabled)
        {
            //  Called when TextDocument.HasChanges is updated
            //  Disables save button if HasChanges == false. Enables it if HasChanges == true
            SaveButton.IsEnabled = enabled;
            //  Uses Italics to show user file has unsaved changes (applied to the document Title)
            FileName.FontStyle = enabled ? FontStyle.Italic : FontStyle.Normal;
            //  If the NewButton is disabled (meaning the document is New) enables it. Can only be disabled by the New() method
            if (!NewButton.IsEnabled) NewButton.IsEnabled = enabled;
        }

        private void InputBox_ClipboardAction(object sender, object args)
        {
            //  Fired when text is altered by Pasting or Cutting text.
            //  This method exists because with the standard TextChanged event Opening a document would cause the event to fire and it would be flagged as needing to be saved.
            //  Now it is only flagged when the user types text, but that prevents mouse based input, mainly pasting and cutting.
            doc.HasChanges = true;
        }
    }
}
