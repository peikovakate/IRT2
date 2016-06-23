using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IRTDocs2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        List<IrtImage> images;
        string url;
        private static string link1 = "ms-appx:///nature.jpeg";
        private static string link2 = "ms-appx:///colors.jpg";
        private static string link3 = "ms-appx:///sailboat.jpg";
        int i = 0;
        private CanvasBitmap qrBitmap;
        QrAnimation qrAnim;
        string token;


        private Color c1 = Color.FromArgb(255, 56, 128, 175);
        private Color c2 = Color.FromArgb(255, 217, 187, 249);
        private Color c3 = Color.FromArgb(255, 204, 167, 162);
        private Color c4 = Color.FromArgb(255, 170, 159, 177);
        private Color c5 = Color.FromArgb(255, 120, 113, 170);


        public MainPage()
        {

            this.InitializeComponent();
            images = new List<IrtImage>();

            FileHandler._urls.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(NewURLArrived);
            token = GenerateRandomData();
            string data = "http://irtdocs2.azurewebsites.net/Upload/AsyncUpload/?id=" + token;
            url = "https://chart.googleapis.com/chart?chs=500x500&cht=qr&chl=" + data + "&chld=L|1&choe=UTF-8";
            // productimage.Source = new BitmapImage(new Uri(url, UriKind.Absolute));

            (Application.Current as App)?.Broadcast(new ChatMessage { Username = "register", Message = token });
            FileHandler.token = token;

            qrAnim = new QrAnimation();
        }

        public string GenerateRandomData()
        {
            // Define the length, in bytes, of the buffer.
            uint length = 32;

            // Generate random data and copy it to a buffer.
            IBuffer buffer = CryptographicBuffer.GenerateRandom(length);

            // Encode the buffer to a hexadecimal string (for display).
            string randomHex = CryptographicBuffer.EncodeToHexString(buffer);

            return randomHex;
        }

        public uint GenerateRandomNumber()
        {
            // Generate a random number.
            uint random = CryptographicBuffer.GenerateRandomNumber();
            return random;
        }

        private void NewURLArrived(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

                addImage(FileHandler._urls.Last());

            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                //your code
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddImage2_Click(object sender, RoutedEventArgs e)
        {
            addImage(link2);
        }

        private void AddImage1_Click(object sender, RoutedEventArgs e)
        {
            addImage(link1);
        }
        private void AddImage3_Click(object sender, RoutedEventArgs e)
        {
            addImage(link3);
        }

        private void addImage(string link)
        {
            i++;
            IrtImage image = new IrtImage(BackgroundGrid);
         
            image.loadImage(link);

        }

        private void qrCode_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            qrAnim.draw(sender, args);
        }

        private void qrCode_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }



        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {

            qrBitmap = await CanvasBitmap.LoadAsync(sender, new Uri(url), 160f);

        }

        private void qrCode_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            qrAnim.bitmap = qrBitmap;
            qrAnim.running = true;
        }


        private void Qr_OnDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            qrAnim.draw(sender, args);
        }

        private void Qr_OnCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }


        private void Qr_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            qrAnim.bitmap = qrBitmap;
            qrAnim.running = true;
        }
    }
}
