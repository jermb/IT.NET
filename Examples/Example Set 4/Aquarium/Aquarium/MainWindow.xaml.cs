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

// Random:
// http://msdn.microsoft.com/en-us/library/system.random.aspx

namespace Aquarium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Background background;
        List<Fish> fishes;

        public MainWindow()
        {
            Fish fish;

            InitializeComponent();

            fishes = new List<Fish>();

            background = new Background(backgroundImage);
            background.Load("background1.png");
            bg1ImageRadio.IsChecked = true;

            fish = new Fish(aquarium: aquarium, dispatcher: Dispatcher, leftImage: "fish1_left.png", rightImage: "fish1_right.png");
            fish.Place(x: 300.0, y: 100.0, direction: "left", wait: 50);
            fishes.Add(fish);

            fish = new Fish(aquarium: aquarium, dispatcher: Dispatcher, leftImage: "fish2_left.png", rightImage: "fish2_right.png");
            fish.Place(x: 100.0, y: 250.0, direction: "right", wait: 20);
            fishes.Add(fish);

            fish = new Fish(aquarium: aquarium, dispatcher: Dispatcher, leftImage: "fish3_left.png", rightImage: "fish3_right.png");
            fish.Place(x: 300.0, y: 170.0, direction: "left", wait: 10);
            fishes.Add(fish);
        }

        private void bg1RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            background.Load("background1.png");
        }

        private void bg2RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            background.Load("background2.png");
        }

        private void bg3RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            background.Load("background3.png");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Fish fish in fishes)
            {
                fish.Shutdown();
            }
        }
    }
}
