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

using System.Threading;

// Threading Model
// http://msdn.microsoft.com/en-us/library/ms741870.aspx
// System.Windows.Threading - Dispatcher
// http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatcher.aspx


// http://www.codeproject.com/KB/WPF/BeginWPF1.aspx

namespace Toy1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int height = 2;
        int width = 2;
        double left = 10.0;
        double top = 100.0;
        Boolean grow = true;
        Boolean forward = true;
        Thread sizeThread = null;
        Thread positionThread = null;
        delegate void myAction ();
          
        public MainWindow()
        {
            InitializeComponent();

            sizeThread = new Thread(new ThreadStart ( Size));
            sizeThread.Start();

            positionThread = new Thread(Position);
            positionThread.Start();
        }

        public void Size()
        {
            while (true)
            {
                if (grow)
                {
                    height++;
                    width++;

                    if (height > 200)
                    {
                        grow = false;
                    }
                }
                else
                {
                    height--;
                    width--;

                    if (height < 3)
                    {
                        grow = true;
                    }
                }

                //ellipse1.Height = height;
                UpdateSize(height, width, ellipse1);
                Thread.Sleep(10);
            }
        }

        void Position()
        {
            while (true)
            {
                if (forward)
                {
                    left += 2.0;

                    if (left > 470.0)
                    {
                        forward = false;
                    }
                }
                else
                {
                    left -= 2.0;

                    if (left < 2.0)
                    {
                        forward = true;
                    }
                }

                UpdatePosition(left, top, ellipse1);
                Thread.Sleep(10);
            }
        }

        void UpdateSize(int h, int w, Ellipse e)
        {
            myAction action = () => { e.Height = h; e.Width = w; };
            
            Dispatcher.BeginInvoke(action);
        }

        void UpdatePosition(double l, double t, Ellipse e)
        {
            Action action = () => { e.SetValue(Canvas.LeftProperty, l); e.SetValue(Canvas.TopProperty, t); };
            Dispatcher.BeginInvoke(action);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sizeThread != null)
            {
                sizeThread.Abort();
            }

            if (positionThread != null)
            {
                positionThread.Abort();
            }
        }


    }
}
