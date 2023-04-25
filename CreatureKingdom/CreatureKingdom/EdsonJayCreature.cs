using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CreatureKingdom
{
    internal class EdsonJayCreature : Creature
    {

        double kingdomWidth;
        double kingdomHeight;

        BitmapImage leftImage;
        BitmapImage rightImage;
        Image currentImage;

        double creatureWidth = 100;
        double increment = 2;

        private Thread? thread;
        private enum Direction { UP, DOWN, LEFT, RIGHT }
        private Direction direction;
        private Random random;

        public EdsonJayCreature(Canvas kingdom, Dispatcher dispatcher, int waitTime = 100) : base(kingdom, dispatcher, waitTime)
        {
            kingdomWidth = kingdom.ActualWidth;
            kingdomHeight = kingdom.ActualHeight;

            currentImage = new Image();
            currentImage.Width = creatureWidth;
            leftImage = LoadBitmap(@"assets/EdsonJay/left.jpg", creatureWidth);
            rightImage = LoadBitmap(@"assets/EdsonJay/left.jpg", creatureWidth);

            random = new Random();
        }

        public override void Place(double x = 100, double y = 200)
        {
            base.Place(x, y);

            direction = (Direction) random.Next(3, 4);

            kingdom.Children.Add(currentImage);
            currentImage.SetValue(Canvas.LeftProperty, x);
            currentImage.SetValue(Canvas.TopProperty, y);

            thread = new Thread(Move);
            thread.Start();
        }

        void Move()
        {
            while (!Paused)
            {
                //int angle = random.Next(1, 4);
                //switch (direction)
                //{
                //    case Direction.UP:
                //        y -= increment;
                //        if (angle == 1) x += increment / 2;
                //        else if (angle == 2) x -= increment / 2;
                //        if (x <)
                //        break;
                //    case Direction.DOWN: y += increment; break;
                //    case Direction.LEFT: x -= increment; break;
                //    case Direction.RIGHT: x += increment; break;
                //}
                switch (direction)
                {
                    case Direction.LEFT: 
                        x -= increment; 
                        if (x < 0)
                        {
                            direction = Direction.RIGHT;
                            ChangeImage(rightImage);
                        }
                        break;
                    case Direction.RIGHT: 
                        x += increment;
                        if (x > kingdom.ActualWidth)
                        {
                            direction = Direction.LEFT;
                            ChangeImage(leftImage);
                        }
                        break;
                }
                Update();
                Thread.Sleep(WaitTime);
            }
        }

        void Update()
        {
            Action action = () => 
            { 
                currentImage.SetValue(Canvas.LeftProperty, x); 
                currentImage.SetValue(Canvas.TopProperty, y); 
            };
            dispatcher.BeginInvoke(action);
        }

        void ChangeImage(BitmapImage image)
        {
            Action action = () => { currentImage.Source = image; };
            dispatcher.BeginInvoke(action);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}
