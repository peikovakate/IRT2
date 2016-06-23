using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using System.Diagnostics;
using System.Numerics;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Core;
using System.Net;

namespace IRTDocs2
{
    class IrtImage
    {
        private CanvasAnimatedControl canvas;
        private OppositeDirection direction;
        private CompositeTransform transform;
        private Point coordinates;
        private CanvasBitmap bitmap;
        private CompositeEffect effect;
        private float blurAmount = 5;
        public int i { get; set; }
        private Grid backgroundGrid;
        private double k = 0.5;
        private double scale = 0.3;
        private string link;
        private AnimatedImage animatedImage;

        public IrtImage(Grid back)
        {
            canvas = new CanvasAnimatedControl();
            direction = new OppositeDirection();
            transform = new CompositeTransform();
            coordinates = new Point(0, 0);

            backgroundGrid = back;
        }

        public void loadImage(string link)
        {
            this.link = link;
            canvas.Draw += Canvas_Draw;
            canvas.CreateResources += Canvas_CreateResources;

            //manipulations
            canvas.ManipulationMode = ManipulationModes.All;
            canvas.ManipulationStarting += Canvas_ManipulationStarting;
            canvas.ManipulationCompleted += Canvas_ManipulationCompleted;
            canvas.ManipulationDelta += Canvas_ManipulationDelta;
            canvas.DoubleTapped += Canvas_DoubleTapped;
            canvas.RenderTransform = transform;
            canvas.RightTapped += Canvas_RightTapped;


            addToBackground();
        }



        private void Canvas_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

            CanvasAnimatedControl c = new CanvasAnimatedControl();
            animatedImage = new AnimatedImage(c, link, backgroundGrid);

            backgroundGrid.Children.Add(c);
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var scaleEffect = new ScaleEffect
            {
                Source = bitmap,
                Scale = new Vector2((float)scale, (float)scale)
            };

            var shadowEffect = new ShadowEffect
            {
                Source = scaleEffect,
                BlurAmount = blurAmount
            };

            effect = new CompositeEffect
            {
                Sources = { shadowEffect, scaleEffect }
            };



            args.DrawingSession.DrawImage(effect, blurAmount * 3, blurAmount * 3);

        }

        private void Canvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {

            transform.ScaleX = 1;
            transform.ScaleY = 1;

        }

        private void Canvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var c = e.Container as CanvasAnimatedControl;
            if (e.Container != null)
            {
                if (Math.Abs(backgroundGrid.ActualWidth / 2) - Math.Abs(transform.TranslateX) - c.Width * transform.ScaleX / 2 < 0.0)
                {

                    direction.X = !direction.X;
                    if (transform.TranslateX < 0)
                    {
                        transform.TranslateX -= Math.Abs(backgroundGrid.ActualWidth / 2) - Math.Abs(transform.TranslateX) - c.Width * transform.ScaleX / 2;
                    }
                    else
                    {
                        transform.TranslateX += Math.Abs(backgroundGrid.ActualWidth / 2) - Math.Abs(transform.TranslateX) - c.Width * transform.ScaleX / 2;
                    }


                }

                if (Math.Abs(backgroundGrid.ActualHeight / 2) - Math.Abs(transform.TranslateY) - c.Height * transform.ScaleY / 2 < 0)
                {
                    direction.Y = !direction.Y;
                    if (transform.TranslateY < 0)
                    {
                        transform.TranslateY -= Math.Abs(backgroundGrid.ActualHeight / 2) - Math.Abs(transform.TranslateY) - c.Height * transform.ScaleY / 2;
                    }
                    else
                    {
                        transform.TranslateY += Math.Abs(backgroundGrid.ActualHeight / 2) - Math.Abs(transform.TranslateY) - c.Height * transform.ScaleY / 2;
                    }

                }
                var tX = e.Delta.Translation.X;
                var tY = e.Delta.Translation.Y;
                if (e.IsInertial)
                {
                    tX *= k;
                    tY *= k;
                }

                if (!direction.X)
                {
                    transform.TranslateX += tX;
                }
                else
                {
                    transform.TranslateX -= tX;
                }

                if (!direction.Y)
                {
                    transform.TranslateY += tY;
                }
                else
                {
                    transform.TranslateY -= tY;
                }

                if (transform.ScaleX * e.Delta.Scale >= 0.2 && transform.ScaleX * e.Delta.Scale <= 4)
                {
                    transform.ScaleX *= e.Delta.Scale;
                    transform.ScaleY *= e.Delta.Scale;


                }
                transform.Rotation += e.Delta.Rotation;
            }
        }

        private void Canvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

            if (e.Container != null)
            {
                direction.X = false;
                direction.Y = false;
            }


        }

        private void Canvas_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            Canvas.SetZIndex(e.Container, backgroundGrid.Children.Count - 1);
            foreach (var item in backgroundGrid.Children)
            {
                Canvas.SetZIndex(item, Canvas.GetZIndex(item) - 1);
            }
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            bitmap = await CanvasBitmap.LoadAsync(sender, new Uri(link));
            //bitmap = CanvasBitmap.CreateFromBytes(sender, ms.ToArray(), 1, 1, Windows.Graphics.DirectX.DirectXPixelFormat.R8G8Int);
            canvas.Width = bitmap.Size.Width * scale + blurAmount * 6;
            canvas.Height = bitmap.Size.Height * scale + blurAmount * 6;
            transform.CenterX = canvas.Width / 2;
            transform.CenterY = canvas.Height / 2;
        }

        private void addToBackground()
        {
            backgroundGrid.Children.Add(canvas);
        }


    }

}