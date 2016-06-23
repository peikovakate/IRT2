using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace IRTDocs2
{
    class QrAnimation
    {
        public List<CanvasBitmap> parts { get; set; }
        public bool running { get; set; }
        public int i { get; set; }
        string link { get; set; }
        public CanvasBitmap bitmap { get; set; }
        public CanvasBitmap image { get; set; }
        private int step = 10;
        private int k = 20;
        Color c1 = Color.FromArgb(255, 56, 128, 175);
        Color c2 = Color.FromArgb(255, 217, 187, 249);
        Color c3 = Color.FromArgb(255, 204, 167, 162);
        Color c4 = Color.FromArgb(255, 170, 159, 177);
        Color c5 = Color.FromArgb(255, 120, 113, 170);

        private int j = 0;

        public bool loaded = false;

        public QrAnimation()
        {
            running = false;
        }

        public void draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            CanvasCommandList cl = new CanvasCommandList(sender);
            CanvasDrawingSession clds = cl.CreateDrawingSession();

            if (loaded)
            {
                j++;
                if (j < step)
                {
                    clds.DrawImage(bitmap);
                }
                else if (j < k + step)
                {
                    Transform3DEffect rotation = new Transform3DEffect()
                    {
                        Source = bitmap,
                        TransformMatrix = Matrix4x4.CreateRotationY((j - step) * (float)Math.PI / 2 / k, new Vector3(150, 150, 0))
                    };
                    clds.DrawImage(rotation);

                }
                else if (j < k * 2 + step)
                {

                    ColorSourceEffect s = new ColorSourceEffect()
                    {
                        Color = Colors.Bisque,

                    };
                    CropEffect crop = new CropEffect()
                    {
                        Source = s,
                        SourceRectangle = new Rect(0, 0, 300, 300)
                    };

                    ShadowEffect shadow = new ShadowEffect()
                    {
                        Source = crop
                    };

                    CompositeEffect composition = new CompositeEffect();
                    composition.Sources.Add(shadow);
                    composition.Sources.Add(crop);

                    Transform3DEffect rotation = new Transform3DEffect()
                    {
                        Source = composition,
                        TransformMatrix = Matrix4x4.CreateRotationY((j - step) * (float)Math.PI / 2 / k, new Vector3(150, 150, 0))
                    };
                    clds.DrawImage(rotation);

                }
                else
                {
                    ColorSourceEffect s = new ColorSourceEffect()
                    {
                        Color = Colors.Bisque,

                    };
                    CropEffect crop = new CropEffect()
                    {
                        Source = s,
                        SourceRectangle = new Rect(0, 0, 300, 300)
                    };

                    ShadowEffect shadow = new ShadowEffect()
                    {
                        Source = crop
                    };

                    CompositeEffect composition = new CompositeEffect();
                    composition.Sources.Add(shadow);
                    composition.Sources.Add(crop);

                    clds.DrawImage(composition);
                }

            }

            else if (!running)
            {
                clds.FillCircle(new Vector2(150), 170, c2);
                clds.FillRectangle(new Rect(0, 100, 300, 100), Colors.White);
                clds.FillRectangle(new Rect(100, 0, 100, 300), Colors.White);

            }
            else
            {
                i++;
                CropEffect firstPart = new CropEffect()
                {
                    Source = bitmap,
                    SourceRectangle = new Rect(0, 100, 300, 100)
                };
                CropEffect secondPart = new CropEffect()
                {
                    Source = bitmap,
                    SourceRectangle = new Rect(100, 0, 100, 300)
                };
                CropEffect topRight = new CropEffect()
                {
                    Source = bitmap,
                    SourceRectangle = new Rect(0, 0, 100, 100)
                };

                CompositeEffect qrPlus = new CompositeEffect();
                qrPlus.Sources.Add(firstPart);
                qrPlus.Sources.Add(secondPart);

                CropEffect buttomRight = new CropEffect()
                {
                    Source = bitmap,
                    SourceRectangle = new Rect(0, 200, 100, 100)
                };

                CropEffect topLeft = new CropEffect()
                {
                    Source = bitmap,
                    SourceRectangle = new Rect(200, 0, 100, 100)
                };

                CropEffect buttomLeft = new CropEffect()
                {
                    Source = bitmap,
                    SourceRectangle = new Rect(200, 200, 100, 100)
                };


                Transform2DEffect rotation;


                if (i < step)
                {

                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation(0)
                    };


                }
                else if (i < k + step)
                {
                    qrPlus.Sources.Add(topRight);

                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((i - step) * (float)Math.PI / k / 2, new Vector2(150, 150))
                    };

                }
                else if (i < k + step * 2)
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((float)Math.PI / 2, new Vector2(150, 150))
                    };
                }
                else if (i < k * 2 + step * 2)
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((i - step * 2) * (float)Math.PI / k / 2, new Vector2(150, 150))
                    };
                }
                else if (i < k * 2 + step * 3)
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    qrPlus.Sources.Add(buttomLeft);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((float)Math.PI, new Vector2(150, 150))
                    };
                }
                else if (i < k * 3 + step * 3)
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    qrPlus.Sources.Add(buttomLeft);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((i - step * 3) * (float)Math.PI / k / 2, new Vector2(150, 150))
                    };
                }
                else if (i < k * 3 + step * 4)
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    qrPlus.Sources.Add(buttomLeft);
                    qrPlus.Sources.Add(topLeft);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((float)Math.PI * 3 / 2, new Vector2(150, 150))
                    };
                }
                else if (i < k * 4 + step * 4)
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    qrPlus.Sources.Add(buttomLeft);
                    qrPlus.Sources.Add(topLeft);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation((i - step * 4) * (float)Math.PI / k / 2, new Vector2(150, 150))
                    };
                }
                else
                {
                    qrPlus.Sources.Add(topRight);
                    qrPlus.Sources.Add(buttomRight);
                    qrPlus.Sources.Add(buttomLeft);
                    qrPlus.Sources.Add(topLeft);
                    rotation = new Transform2DEffect()
                    {
                        Source = qrPlus,
                        TransformMatrix = Matrix3x2.CreateRotation(0, new Vector2(150, 150))
                    };
                }

                clds.DrawImage(rotation);

            }
            args.DrawingSession.DrawImage(cl, new Vector2(62, 62));
        }
    }
}
