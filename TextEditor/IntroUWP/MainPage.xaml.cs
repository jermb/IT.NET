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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IntroUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private  void HelloBtn_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = new MessageDialog("Hi!");
            //await dialog.ShowAsync();

            Display.Text = "Hello World!";
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {


            Display.Text = "Open";

            Display.Text = "";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                Display.Text = "Picked photo: " + file.Name;
            }
            else
            {
                Display.Text = "Operation cancelled.";
            }

        //https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files
            string text = await Windows.Storage.FileIO.ReadTextAsync(file);
            Display.Text = text;


        }
    }
}
