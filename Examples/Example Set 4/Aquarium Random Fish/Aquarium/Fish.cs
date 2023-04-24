using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Threading;
using System.Windows.Threading;

namespace Aquarium
{
    class Fish
    {
        Canvas aquarium;
        Image fishImage;
        BitmapImage leftBitmap;
        BitmapImage rightBitmap;
        double aquariumWidth = 0.0;
        double fishWidth = 100.0;
        double maxX = 0.0;
        double incrementSize = 2.0;

        private double y;
        private double x;
        private Dispatcher dispatcher;
        private Int32 waitTime;
        private Boolean goRight = true;
        private Thread posnThread = null;


        public Fish(Canvas aquarium, Dispatcher dispatcher, String leftImage, String rightImage)
        {
            this.aquarium = aquarium;
            this.dispatcher = dispatcher;
            aquariumWidth = (int)this.aquarium.Width;
            maxX = aquariumWidth - fishWidth;

            fishImage = new Image();
            fishImage.Width = fishWidth;

            leftBitmap = LoadBitmap(leftImage);
            rightBitmap = LoadBitmap(rightImage);
        }

        private BitmapImage LoadBitmap(String imageFile)
        {
            BitmapImage theBitmap = new BitmapImage();
            theBitmap.BeginInit();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, imageFile);
            theBitmap.UriSource = new Uri(path, UriKind.Absolute);
            theBitmap.DecodePixelWidth = (int)fishWidth;
            theBitmap.EndInit();

            return theBitmap;
        }

        public void Place(double x = 100.0, double y = 200.0, String direction = "right", Int32 wait = 100)
        {
            switch (direction)
            {
                case "right":
                {
                    fishImage.Source = rightBitmap;
                    goRight = true;
                    break;
                }
                case "left":
                {
                    fishImage.Source = leftBitmap;
                    goRight = false;
                    break;
                }
                default:
                {
                    fishImage.Source = rightBitmap;
                    goRight = true;
                    break;
                }
            }

            this.waitTime = wait;
            this.x = x;
            this.y = y;
            aquarium.Children.Add(fishImage);
            fishImage.SetValue(Canvas.LeftProperty, this.x);
            fishImage.SetValue(Canvas.TopProperty, this.y);

            posnThread = new Thread(Position);

            posnThread.Start();
        }

        void Position()
        {
            while (true)
            {
                if (goRight)
                {
                    x += incrementSize;

                    if (x > maxX)
                    {
                        goRight = false;
                        SwitchBitmap(leftBitmap);
                    }
                }
                else
                {
                    x -= incrementSize;

                    if (x < 0)
                    {
                        goRight = true;
                        SwitchBitmap(rightBitmap);
                    }
                }

                UpdatePosition();
                Thread.Sleep(waitTime);
            }
        }

        void UpdatePosition()
        {
            Action action = () => { fishImage.SetValue(Canvas.LeftProperty, x);fishImage.SetValue(Canvas.TopProperty, y); };
            dispatcher.BeginInvoke(action);
        }

        void SwitchBitmap(BitmapImage theBitmap)
        {
            Action action = () => { fishImage.Source = theBitmap; };
            dispatcher.BeginInvoke(action);
        }

        public void Shutdown()
        {
            if (posnThread != null)
            {
                posnThread.Abort();
            }
        }
    }
}
