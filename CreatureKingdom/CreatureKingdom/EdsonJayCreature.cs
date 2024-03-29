﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        BitmapImage upImage;
        BitmapImage downImage;
        Image currentImage;

        double creatureWidth = 100;
        double increment = 20;

        private Thread? thread;
        private bool running = false;
        private enum Direction { UP, DOWN, LEFT, RIGHT }
        private Direction direction;
        private Random random;

        public EdsonJayCreature(Canvas kingdom, Dispatcher dispatcher, int waitTime = 100) : base(kingdom, dispatcher, waitTime)
        {
            kingdomWidth = kingdom.ActualWidth;
            kingdomHeight = kingdom.ActualHeight;

            currentImage = new Image();
            currentImage.Width = creatureWidth;
            leftImage = LoadBitmap(@"EdsonJay/left.png", creatureWidth);
            rightImage = LoadBitmap(@"EdsonJay/right.png", creatureWidth);
            upImage = LoadBitmap(@"EdsonJay/up.png", creatureWidth);
            downImage = LoadBitmap(@"EdsonJay/down.png", creatureWidth);

            random = new Random();
        }

        public override void Place(double x = 100, double y = 200)
        {
            base.Place(x, y);

            direction = (Direction) random.Next(0, 3);

            switch (direction)
            {
                case Direction.UP: currentImage.Source = upImage; break;
                case Direction.DOWN: currentImage.Source = downImage; break;
                case Direction.LEFT: currentImage.Source = leftImage; break;
                case Direction.RIGHT: currentImage.Source = rightImage; break;
            }
            kingdom.Children.Add(currentImage);
            currentImage.SetValue(Canvas.LeftProperty, x);
            currentImage.SetValue(Canvas.TopProperty, y);

            running = true;
            thread = new Thread(Move);
            thread.Start();
        }

        void Move()
        {
            while (running)
            {
                if (!Paused)
                {
                    int angle = random.Next(1, 3);
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
                            if (x > kingdom.ActualWidth - currentImage.ActualWidth)
                            {
                                direction = Direction.LEFT;
                                ChangeImage(leftImage);
                            }
                            break;
                        case Direction.UP:
                            y -= increment;
                            if (y < 0)
                            {
                                direction = Direction.DOWN;
                                ChangeImage(downImage);
                            }
                            break;
                        case Direction.DOWN:
                            y += increment;
                            if (y > kingdom.ActualHeight - currentImage.ActualHeight)
                            {
                                direction = Direction.UP;
                                ChangeImage(upImage);
                            }
                            break;
                    }

                    switch (direction)
                    {
                        case Direction.LEFT:
                        case Direction.RIGHT:
                            if (angle == 1)
                            {
                                y += increment;
                                if (y > kingdom.ActualHeight - currentImage.ActualHeight)
                                {
                                    direction = Direction.UP;
                                    ChangeImage(upImage);
                                }
                            }
                            else if (angle == 2)
                            {
                                y -= increment;
                                if (y < 0)
                                {
                                    direction = Direction.DOWN;
                                    ChangeImage(downImage);
                                }
                            }
                            break;
                        case Direction.UP:
                        case Direction.DOWN:
                            if (angle == 1)
                            {
                                x += increment;
                                if (x > kingdom.ActualWidth - currentImage.ActualWidth)
                                {
                                    direction = Direction.LEFT;
                                    ChangeImage(leftImage);
                                }
                            }
                            else if (angle == 2)
                            {
                                x -= increment;
                                if (x < 0)
                                {
                                    direction = Direction.RIGHT;
                                    ChangeImage(rightImage);
                                }
                            }
                            break;
                    }

                    Update();
                }
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
            Paused = true;
            running = false;
            if (thread != null)
            {
                thread.Join();
            }
        }
    }
}
