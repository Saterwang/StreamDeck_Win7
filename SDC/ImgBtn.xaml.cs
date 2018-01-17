using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace SDC
{
    /// <summary>
    /// ImgBtn.xaml 的交互逻辑
    /// </summary>
    public partial class ImgBtn : UserControl
    {
        public static readonly RoutedEvent MyButtonClickEvent = EventManager.RegisterRoutedEvent("MyButtonClick", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(ImgBtn));

        /// <summary>
        /// Select Status
        /// </summary>
        private bool isSelected = false;
        public bool Selected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                brdSelect.Visibility = isSelected ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Button Index in ButtonMatrix Panel, also in StreamDeck
        /// </summary>
        public int Index
        { get; set; }

        private bool isClicking = false;

        public ImgBtn()
        {
            InitializeComponent();
        }

        public event RoutedPropertyChangedEventHandler<object> MyButtonClick
        {
            add
            {
                this.AddHandler(MyButtonClickEvent, value);
            }

            remove
            {
                this.RemoveHandler(MyButtonClickEvent, value);
            }
        }

        public void OnMyButtonClick(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, MyButtonClickEvent);

            this.RaiseEvent(arg);
        }

        private void ImageBlock_Drop(object sender, DragEventArgs e)
        {
            // 图片文件
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                BitmapImage img = new BitmapImage();
                //若要原始文件的站点，可以调用 Application 类的 GetRemoteStream 方法，同时传递标识原始文件的所需站点的 pack URI。 GetRemoteStream 将返回一个 StreamResourceInfo 对象，该对象将原始文件的该站点作为 Stream 公开，并描述其内容类型。
                img.BeginInit();
                try
                {
                    string filepath = files[0].ToString();
                    Stream sr = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    img.StreamSource = sr;
                }
                catch(Exception ex)
                {

                }
                
                img.EndInit();
                (this.DataContext as ButtonData).Image = img;

                //ImageBlock.Source = img;
            }

            // ImgBtn Move or Exchange
            if (e.Data.GetDataPresent(typeof(ImgBtn)))
            {
                ImgBtn dropSrc = e.Data.GetData(typeof(ImgBtn)) as ImgBtn;

                if (dropSrc != null)
                {
                    int SrcIndex = dropSrc.Index;
                    int TarIndex = this.Index;

                    PageData currPage = Business.GetInstance().ButtonMatrixPanel.DataContext as PageData;

                    ButtonData temp = currPage.BtnList[SrcIndex];
                    currPage.BtnList[SrcIndex] = currPage.BtnList[TarIndex];
                    currPage.BtnList[TarIndex] = temp;
                    Business.GetInstance().ButtonMatrixPanel.InitPanel();
                }
            
            }
                // 快捷方式
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
    
               
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isClicking = true;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(isClicking)
            {
                isClicking = false;

            }

            OnMyButtonClick(true, true);
        }

        public Bitmap GetRenderBitmap(bool isWithBorder = false)
        {

            BitmapSource b;
            if(isWithBorder)
            {
                b = RenderUIElement(CoreBorder, 72, 72, 96, 96);
            }
            else
            {
                b = RenderUIElement(CoreGrid, 72, 72, 96, 96);
            }
            
            Bitmap bmp = GetBitmap(b);
            return bmp;
        }
        private Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
        private static BitmapSource RenderUIElement(Visual target, double width, double height, double dpiX, double dpiY)
        {
            if (target == null)
            {
                return null;
            }

            Rect bounds = new Rect(0, 0, width, height);

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                                            (int)(bounds.Height * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }

            rtb.Render(dv);

            return rtb;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            Business.GetInstance().RefreshStreamDeckButton(Index);
        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            var dependencyObject = (ImgBtn)sender;

            if (dependencyObject != null && dependencyObject.IsMouseOver && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(dependencyObject, dependencyObject, DragDropEffects.Move);
            }
        }
    }
}
