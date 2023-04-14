using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

using System.IO;

// image came from:
// http://www.iconseeker.com/png/art-toys/blue-toy.html

namespace Playland
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] filepaths;
        //  Set to negative one so NextImage() can be used to initialize image
        private int index = -1;
        public MainWindow()
        {
            InitializeComponent();
            //  Grabs the file path of each imge in the bin/Debug/Images directory
            filepaths = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, @"Images"));
            //  Displays the first image
            NextImage(null, null);
        }

        private void NextImage(object sender, RoutedEventArgs e)
        {
            index = (index == filepaths.Length - 1) ? 0 : (index + 1);
            //  Creates an image using the file path
            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filepaths[index]);
            bitmap.DecodePixelWidth = 384;

            //  Prevents program from crashing if a non-image file is within the directory
            try { bitmap.EndInit(); }
            catch (NotSupportedException)
            {
                //  Move on to the next image
                NextImage(sender, e);
                return;
            }

            image.Source = bitmap;

            ImageDisplay.Children.Clear();
            ImageDisplay.Children.Add(image);
            Canvas.SetLeft(image, 70);
        }
    }
}
