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
using System.Windows.Threading;

// Threading Model
// http://msdn.microsoft.com/en-us/library/ms741870.aspx
// System.Windows.Threading - Dispatcher
// http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatcher.aspx

namespace Toys1
{
    class Toy
    {
        private double top;
        private double left;
        private int height;
        private int width;
        private double opacity;
        private Int32 waittime;
        private Boolean grow = true;
        private Boolean forward = true;
        private Canvas canv;
        private Ellipse elips;
        private Dispatcher disp;
        private Thread sizeThread = null;
        private Thread posnThread = null;


        public Toy(Canvas c, Dispatcher d, double t = 0, double l = 0, int h = 2, int w = 2, double opacity = 1.0, Int32 wtime = 10)
        {
            canv = c;
            disp = d;
            top = t;
            left = l;
            height = h;
            width = w;
            this.opacity = opacity;
            waittime = wtime;

            elips = new Ellipse();
            elips.Stroke = System.Windows.Media.Brushes.Black;
            elips.Fill = System.Windows.Media.Brushes.DarkBlue;
            //elips.HorizontalAlignment = HorizontalAlignment.Left;
            //elips.VerticalAlignment = VerticalAlignment.Center;
            elips.Width = width;
            elips.Height = height;
            elips.Opacity = this.opacity;
            canv.Children.Add(elips);

            posnThread = new Thread(Size);
            sizeThread = new Thread(Position);

            posnThread.Start();
            sizeThread.Start();

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

                UpdateSize();
                Thread.Sleep(waittime);
            }
        }

        void Position()
        {
            while (true)
            {
                if (forward)
                {
                    left += 2.0;

                    if (left > 440.0)
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

                UpdatePosition();
                Thread.Sleep(waittime);
            }
        }

        void UpdateSize()
        {
            Action action = () => { elips.Height = height; elips.Width = width; };
            disp.BeginInvoke(action);
        }

        void UpdatePosition()
        {
            Action action = () => { elips.SetValue(Canvas.LeftProperty, left); elips.SetValue(Canvas.TopProperty, top); };
            disp.BeginInvoke(action);
        }

        public void Shutdown()
        {
            if (posnThread != null) 
            {
                posnThread.Abort();
            }

            if (sizeThread != null) 
            {
                sizeThread.Abort();
            }
        }

    }
}