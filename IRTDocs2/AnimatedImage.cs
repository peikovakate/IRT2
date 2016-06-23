using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input;
using System.Runtime.InteropServices;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using System.Collections.Concurrent;

namespace IRTDocs2
{
    internal class AnimatedImage
    {
        CanvasAnimatedControl canvasAnimated;
        String link;

        List<Color> colors;

        CanvasRenderTarget[] bitmapsArr;
        ConcurrentDictionary<uint, MyPoint> touches;

        Grid background;
        double sensivity = 6;
        int amount = 20;
        float r = 3f;

        int pointCount = 0;
        IntPtr startPoints;
        IntPtr currentPoints;



        float left = 150;
        float top = 150;

        public AnimatedImage(CanvasAnimatedControl canvas, string link, Grid background)
        {
            this.canvasAnimated = canvas;

            this.link = link;
            this.background = background;

            colors = new List<Color>();
            touches = new ConcurrentDictionary<uint, MyPoint>();



            Point mousePosition = new Point(0, 0);
            canvasAnimated.Draw += CanvasAnimated_Draw;
            canvasAnimated.CreateResources += CanvasAnimated_CreateResources;

            background.PointerMoved += Background_PointerMoved;
            background.PointerPressed += Background_PointerPressed;
            background.PointerReleased += Background_PointerReleased;
        }



        private void CanvasAnimated_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {

            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());

        }

        public struct MyPoint
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        int pointIndex = 0;
        MyPoint originalPoint;
        MyPoint currentPoint;

        float theta;
        float distance;


        unsafe
        private void CanvasAnimated_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            MyPoint* pnts = (MyPoint*)startPoints.ToPointer();
            MyPoint* pntc = (MyPoint*)currentPoints.ToPointer();


            using (var sprite = args.DrawingSession.CreateSpriteBatch())
            {
                for (pointIndex = 0; pointIndex < pointCount; pointIndex++)
                {
                    originalPoint = *(pnts + pointIndex);
                    currentPoint = *(pntc + pointIndex);
                    theta = 0;
                    distance = 0;
                    float maxDist = 0;
                    float maxTheta = 0;
                    foreach (var mousePosition in touches.Values)
                    {
                        theta = (float)Math.Atan2(currentPoint.Y - mousePosition.Y, currentPoint.X - mousePosition.X);
                        distance = (float)(sensivity * 100 / Math.Sqrt((mousePosition.X - currentPoint.X) * (mousePosition.X - currentPoint.X)
                            + (mousePosition.Y - currentPoint.Y) * (mousePosition.Y - currentPoint.Y)));
                        if (distance >= maxDist)
                        {
                            maxDist = distance;
                            maxTheta = theta;
                        }
                    }

                    theta = maxTheta;
                    distance = maxDist;

                    currentPoint.X += (float)(Math.Cos(theta) * distance + (originalPoint.X - currentPoint.X) * 0.05);
                    currentPoint.Y += (float)(Math.Sin(theta) * distance + (originalPoint.Y - currentPoint.Y) * 0.05);

                    (*(pntc + pointIndex)).X = currentPoint.X;
                    (*(pntc + pointIndex)).Y = currentPoint.Y;

                    sprite.Draw(circle, new Vector2(currentPoint.X, currentPoint.Y));
                }


            }
        }


        double R, G, B;
        int stepX = 0;
        int stepY = 0;
        int index;


        CanvasRenderTarget circle;

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            var random = await RandomAccessStreamReference.CreateFromUri(new Uri(link)).OpenReadAsync();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(random);
            PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            var vect = new Vector2(r, r);

            byte[] bytes = pixelData.DetachPixelData();
            index = 0;
            pointCount = bytes.Length / 4 / amount / amount;
            bitmapsArr = new CanvasRenderTarget[pointCount];
            startPoints = Marshal.AllocHGlobal(pointCount * 8);
            currentPoints = Marshal.AllocHGlobal(pointCount * 8);

            circle = new CanvasRenderTarget(sender, 2 * r, 2 * r, 96);
            circle.CreateDrawingSession().FillCircle(vect, r, Colors.Black);

            unsafe
            {
                MyPoint* pnts = (MyPoint*)startPoints.ToPointer();
                MyPoint* pntc = (MyPoint*)currentPoints.ToPointer();



                for (int i = 0; i < decoder.PixelHeight; i += amount)
                {
                    stepX++;
                    stepY = 0;
                    for (int j = 0; j < decoder.PixelWidth; j += amount)
                    {
                        stepY++;
                        R = G = B = 0;

                        for (int g = 0; g < amount && (i + g) < decoder.PixelHeight; g++)
                        {
                            for (int k = 0; k < amount && (j + k) < decoder.PixelWidth; k++)
                            {
                                R += (bytes[i * decoder.PixelWidth * 4 + g * decoder.PixelWidth * 4 + j * 4 + k * 4 + 2] / (double)amount / amount);
                                G += (bytes[i * decoder.PixelWidth * 4 + j * 4 + k * 4 + g * decoder.PixelWidth * 4 + 1] / (double)amount / amount);
                                B += (bytes[i * decoder.PixelWidth * 4 + j * 4 + k * 4 + g * decoder.PixelWidth * 4] / (double)amount / amount);
                            }
                        }

                        Color c = Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
                        colors.Add(c);

                        if (R > 10 && G > 10 && B > 10)
                        {

                            continue;
                        }

                        //так делать очень плохо, Катя
                        if (index >= pointCount)
                        {
                            pointCount = index;
                            return;
                        }


                        bitmapsArr[index] = new CanvasRenderTarget(sender, 2 * r, 2 * r, 96);
                        bitmapsArr[index].CreateDrawingSession().FillCircle(vect, r, c);


                        float x = (float)(j / amount * r * 2 + r + stepY) + top;
                        float y = (float)(i / amount * r * 2 + r + stepX) + left;




                        (*(pnts + index)).X = x;
                        (*(pnts + index)).Y = y;
                        (*(pntc + index)).X = x;
                        (*(pntc + index)).Y = y;
                        index++;
                    }
                }
                pointCount = index;
            }
        }

        private void Background_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //var mousePointer = e.GetCurrentPoint(canvasAnimated);
            //mousePosition.X = (float)mousePointer.Position.X;
            //mousePosition.Y = (float)mousePointer.Position.Y;


            MyPoint point = new MyPoint();
            point.X = (float)e.GetCurrentPoint(canvasAnimated).Position.X;
            point.Y = (float)e.GetCurrentPoint(canvasAnimated).Position.Y;

            if (touches.Keys.ToList().IndexOf(e.Pointer.PointerId) != -1)
            {
                touches[e.Pointer.PointerId] = point;
            }

        }
        private void Background_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            MyPoint p;
            touches.TryRemove(e.Pointer.PointerId, out p);

        }

        private void Background_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            MyPoint point = new MyPoint
            {
                X = (float)e.GetCurrentPoint(canvasAnimated).Position.X,
                Y = (float)e.GetCurrentPoint(canvasAnimated).Position.Y
            };
            touches[e.Pointer.PointerId] = point;

        }

    }

}
