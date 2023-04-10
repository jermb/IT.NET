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
        public MainWindow()
        {
            InitializeComponent();

            Image myImage = new Image();
            myImage.Width = 128;
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"blue-toy.png");
            // Create source
            BitmapImage myBitmapImage = new BitmapImage();
            //BitmapImage myBitmapImage = new BitmapImage( new Uri (path));

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();

            //// Path.Combine requires using System.IO
            //// http://stackoverflow.com/questions/5560653/image-source-does-not-locate-image-file-in-working-directory
            //// "The problem is that without the full path, WPF assumes it is a relative path to an embedded resource, not a file on disk."
            //string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"blue-toy.png");
            //// If you remove System.IO you can experience the issue of a conflict between the same named thing in two different name spaces.
            //// For use of @"" see: http://msdn.microsoft.com/en-us/library/vstudio/362314fe.aspx

            myBitmapImage.UriSource = new Uri(path);

            //// To save significant application memory, set the DecodePixelWidth or  
            //// DecodePixelHeight of the BitmapImage value of the image source to the desired 
            //// height or width of the rendered image. If you don't do this, the application will 
            //// cache the image as though it were rendered as its normal size rather then just 
            //// the size that is displayed.
            //// Note: In order to preserve aspect ratio, set DecodePixelWidth
            //// or DecodePixelHeight but not both.
            myBitmapImage.DecodePixelWidth = 128;
            myBitmapImage.EndInit();
            //set image source
            myImage.Source = myBitmapImage;

            canvas1.Children.Add(myImage);
            Canvas.SetLeft(myImage, 150);
        }
    }
}
