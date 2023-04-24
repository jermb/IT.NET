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

namespace Toys1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Toy[] toys; 

        public MainWindow()
        {
            InitializeComponent();

            toys =  new Toy[] {
                new Toy(playpen, Dispatcher, t:10, l:0, opacity:0.5, wtime:20),
                new Toy(playpen, Dispatcher, t:60, l:40, opacity:0.4),
                new Toy(playpen, Dispatcher, t:80, l:60, opacity:0.3, wtime:15),
                new Toy(playpen, Dispatcher, t:90, l:0, opacity:0.6, wtime:5),
                new Toy(playpen, Dispatcher, t:100, l:100, opacity:0.4, wtime:9),
                new Toy(playpen, Dispatcher, t:50, l:200, opacity:0.4, wtime:12),
                new Toy(playpen, Dispatcher, t:65, l:10, opacity:0.4, wtime:4),
                new Toy(playpen, Dispatcher, t:77, l:220, opacity:0.4, wtime:11),
                new Toy(playpen, Dispatcher, t:130, l:300, opacity:0.4, wtime:17),
                new Toy(playpen, Dispatcher, t:20, l:100, opacity:0.4, wtime:18),
                new Toy(playpen, Dispatcher, t:130, l:200, opacity:0.4, wtime:6),
                new Toy(playpen, Dispatcher, t:140, l:50, opacity:0.4, wtime:30),
            };

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Toy toy in toys)
            {
                toy.Shutdown();
            }
        }

    }
}
