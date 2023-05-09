using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Text;
using System.Diagnostics;
using Windows.System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI;
using static System.Net.Mime.MediaTypeNames;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PythonIDE
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
 
    public sealed partial class MainPage : Page
    {
        private static TextBlock output;
        private static TextBlock debug;
        private static MainPage main;

        private PythonFile doc;
        FolderNavigation nav = new FolderNavigation();

        public MainPage()
        {

            this.InitializeComponent();
            Doc = new PythonFile();
            output = Output;
            debug = DebugText;
            nav.Set(Doc);
            main = this;
        }

        /***** Properties *****/
        private PythonFile Doc { get => doc; set { doc = value; FileName.Text = doc.DisplayName; DocText = doc.Contents; } }
        public ITextSelection Selection
        {
            get => RichTextBox.Document.Selection;
        }

        public string DocText
        {
            get { RichTextBox.Document.GetText(TextGetOptions.None, out string text); return text; }
            set => RichTextBox.Document.SetText(TextSetOptions.None, (value != null) ? value : "");
        }

        /***** File Handling *****/
        private async void OpenFile(object sender, RoutedEventArgs e)
        {
            //  Make sure the user doesn't unwillingly delete changes
            if (!await ResolveUnsavedChanges("Are you sure?")) return;

            //  Opens a dialog to choose a file
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".py");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return;
            //  If a new file is successfully opened ensures the New document button is available
            NewButton.IsEnabled = true;
            //  Creates and opens python file
            PythonFile pyFile = new PythonFile(file);
            await pyFile.Open();
            //  Sets file values to ui
            FileName.Text = pyFile.DisplayName;
            DocText = pyFile.Contents;
            nav.Set(pyFile);
            Doc = pyFile;
        }

        private async void OpenFolder(object sender, RoutedEventArgs e)
        {
            if (!await ResolveUnsavedChanges("Are you sure?")) return;
            //  Opens folder picker dialog
            FolderPicker picker = new FolderPicker();
            picker.FileTypeFilter.Add(".py");

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder == null) return;

            //  Sets folder as navigation tree folder
            PythonFolder pyFolder = new PythonFolder(folder);
            pyFolder.LoadItems();
            nav.Set(pyFolder);
            Doc = nav.GetFirstFile();
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
            if (!Doc.IsStored) { await SaveAs(); return; }
            //  If it has a name and location, save it quietly
            await Doc.Save();
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
            savePicker.FileTypeChoices.Add("Python Program", new List<string>() { ".py" });
            //  If the Document already has a name fill that into the file name field, otherwise default to 'Untitled'
            savePicker.SuggestedFileName = Doc.DisplayName ?? "Untitled";
            StorageFile file = await savePicker.PickSaveFileAsync();
            // Creates new PythonFile using file object
            Doc = new PythonFile(file);
            //  Saves text into that file
            await Doc.Save();
            FileName.Text = Doc.DisplayName;
        }

        private async void SaveAll(object sender, RoutedEventArgs e)
        {
            await nav.Save();
        }

        private async void New(object sender, RoutedEventArgs e)
        {
            //  If the current doc has Unsaved changes ask the user to save them before opening a new document
            //  If they cancel the operation nothing happens, otherwise a new document is created.
            if (await ResolveUnsavedChanges("Are you sure?"))
            {
                PythonFile file = new PythonFile();
                nav.AddItem(file);
                Doc = file;
            }
            NewButton.IsEnabled = false;
        }


        /***** Menu Bar *****/
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
            await new MessageDialog("This application is a basic Python editor. It is capable of opening, saving, and running '.py' files.\nIt was created by Jay Edson for INFOTC 4400 as the final project.", "About").ShowAsync();
        }

        private bool syntaxColoring = false;
        private void SyntaxColorToggle(object sender, RoutedEventArgs e)
        {
            //  Toggles whether text is dynamically colored
            syntaxColoring = !syntaxColoring;

            if (syntaxColoring)
            {
                RichTextBox.ColorText();
                SyntaxColoringButton.Text = "Syntax Coloring On";
            }
            else
            {
                RichTextBox.NoColor();
                SyntaxColoringButton.Text = "Syntax Coloring Off";
            }
        }

        private bool wordWrapping = true;
        private void WordWrapToggle(object sender, RoutedEventArgs e)
        {
            //  Toggles whether text overflow wraps or scrolls
            wordWrapping = !wordWrapping;

            if (wordWrapping)
            {
                RichTextBox.TextWrapping = TextWrapping.WrapWholeWords;
                WordWrapButton.Text = "Word Wrap On";
            }
            else
            {
                RichTextBox.TextWrapping = TextWrapping.NoWrap;
                WordWrapButton.Text = "Word Wrap Off";
            }
        }

        private void CommentOut(object sender, RoutedEventArgs e)
        {
            RichTextBox.CommentOut();
        }


        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            //  Just copies text 
            if (RichTextBox.Tag != null)
            {
                Tuple<int, int> selection = (Tuple<int, int>)RichTextBox.Tag;
                RichTextBox.Document.Selection.SetRange(selection.Item1, selection.Item2);
                RichTextBox.Document.Selection.Copy();
                RichTextBox_ClipboardAction(sender, e);
            }

        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            //  Pastes text
            if (RichTextBox.Tag != null)
            {
                Tuple<int, int> selection = (Tuple<int, int>)RichTextBox.Tag;
                RichTextBox.Document.Selection.SetRange(selection.Item1, selection.Item2);
                RichTextBox.Document.Selection.Paste(0);
                RichTextBox_ClipboardAction(sender, e);
            }
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            //  Cuts text
            if (RichTextBox.Tag != null)
            {
                Tuple<int, int> selection = (Tuple<int, int>)RichTextBox.Tag;
                RichTextBox.Document.Selection.SetRange(selection.Item1, selection.Item2);
                RichTextBox.Document.Selection.Cut();
                RichTextBox_ClipboardAction(sender, e);
            }
        }

        /*  Utility Methods  */
        private async Task<bool> ResolveUnsavedChanges(string msgTitle, bool discard = true)
        {
            if (!nav.HasChanges()) return true;
            //  Checks whether the user really wants to exit the application/current document with unsaved changes
            var dialog = new MessageDialog("You have unsaved changes.", msgTitle);
            //  Creates buttons and then adds them to the dialog box
            var saveCommand = new UICommand("Save");
            var discardCommand = new UICommand("Discard");
            var cancelCommand = new UICommand("Cancel");
            dialog.Commands.Add(saveCommand);
            if (discard) dialog.Commands.Add(discardCommand);
            dialog.Commands.Add(cancelCommand);

            var command = await dialog.ShowAsync();
            if (command == cancelCommand || (command == discardCommand && !discard))
            {
                //  Cancelling returns false and prevents whatever action was occurring
                return false;
            }
            else if (command == saveCommand)
            {
                await nav.Save();
            }
            //  Saving or discarding changes continues operation
            return true;
        }



        /***** Event Handlers *****/

        private void TabHandling(object sender, KeyRoutedEventArgs e)
        {
            //  This method is technically a KeyDown event, so any time a user types we use that to tell the application the document has been changed
            Doc.HasChanges = true;
            //  Watch for a Tab press and handle as input rather than moving to next navigation item
            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                //  Finds the area selected by the user and replaces it with a Tab character
                //  Allows for replacing multiple characters with one tab, as opposed to just inserting a tab where the cursor is
   
                int start = Selection.StartPosition;
                int length = Selection.Length;
                
                DocText = DocText.Substring(0, start) + "\t" + DocText.Substring(start + length);
  
                //  Moves the cursor behind the new tab
                Selection.StartPosition = ++start;
                //  Absorbs tab press, prevents moving to next navigation item
                e.Handled = true;
            }
        }


        private void RichTextBox_ClipboardAction(object sender, object args)
        {
            //  Fired when text is altered by Pasting or Cutting text.
            //  This method exists because with the standard TextChanged event Opening a document would cause the event to fire and it would be flagged as needing to be saved.
            //  Now it is only flagged when the user types text, but that prevents mouse based input, mainly pasting and cutting.
            Doc.HasChanges = true;
        }


        private void RunProgram(object sender, RoutedEventArgs e)
        {
            //  Calls async function to run python code
            RunProgram();
        }

        private async Task RunProgram()
        {
            if (await ResolveUnsavedChanges("Must save changes before running.", false))
            {
                //  Runs Python code
                Doc.Run();
            }
        }

        private bool ignoreTextChange = false;
        private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            //  Uses variable to prevent formatting of text from creating infinite loop
            if (ignoreTextChange || !syntaxColoring) return;
            ignoreTextChange = true;

            //  Adds color to text of keywords
            RichTextBox.ColorText();
            ignoreTextChange = false;
        }


        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //  Saves the selected text range so menu buttons can still perform functions on that text.
            ITextSelection selection = RichTextBox.Document.Selection;
            RichTextBox.Tag = new Tuple<int, int>(selection.StartPosition, selection.EndPosition);
        }

        private void RichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //  Resets selection range
            if (RichTextBox.Tag != null)
            {
                Tuple<int, int> selection = (Tuple<int, int>)RichTextBox.Tag;
                RichTextBox.Document.Selection.SetRange(selection.Item1, selection.Item2);
            }
        }

        private void TreeViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //  Casts TreeViewItem to Python Item and pass on to async function
            var tvi = sender as TreeViewItem;
            var item = tvi.DataContext as PythonItem;

            TVI_Tapped(item);
        }

        private async Task TVI_Tapped(PythonItem item)
        {
            //  If tapped item is PythonFile sets that file as working file.
            if (item is PythonFile file)
            {
                //  Soft saves changes to current file
                Doc.Contents = DocText;
                //  Sets working file to the tapped file
                Doc = file;
                //  Updates values in UI
                DocText = file.Contents;
                FileName.Text = file.DisplayName;
                UnsavedChanges(file.HasChanges);
            }
        }


        /***** Static Methods *****/
        public static string GetContent()
        {
            return main.DocText;
        }

        public static void UnsavedChanges(bool enabled)
        {
            //  Called when TextDocument.HasChanges is updated
            //  Disables save button if HasChanges == false. Enables it if HasChanges == true
            main.SaveButton.IsEnabled = enabled;
            //  Uses Italics to show user file has unsaved changes (applied to the document Title)
            main.FileName.FontStyle = enabled ? FontStyle.Italic : FontStyle.Normal;
            //  If the NewButton is disabled (meaning the document is New) enables it. Can only be disabled by the New() method
            if (!main.NewButton.IsEnabled) main.NewButton.IsEnabled = enabled;
        }

        public static void SetOutput(string text, string error)
        {
            //  Formats and displays text in Output and Debug Textboxes
            text = "> " + string.Join("\n> ", text.Split('\n'));
            output.Text = text;
            error = "> " + string.Join("\n> ", error.Split('\n'));
            debug.Text = error;
        }
    }
}
