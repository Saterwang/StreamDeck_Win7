using AutoHotkey.Interop;
using StreamDeckSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SDC
{
    public class Business
    {
        private static Object crit = new Object();
        private static Business m_Instance;
        private AutoHotkeyEngine ahk = AutoHotkeyEngine.Instance;
        public static Business GetInstance()
        {
            lock (crit)
            {
                if (m_Instance == null)
                {
                    m_Instance = new Business();
                }
                return m_Instance;
            }
        }

        public IStreamDeck Deck
        {
            get; set;
        }

        public ButtonMatrix ButtonMatrixPanel
        { get; set; }

        public MainWindow MainWindowPanel
        { get; set; }

        private Business()
        {
            try
            {
                Deck = StreamDeck.FromHID();
                Deck.KeyPressed += Deck_KeyPressed;
            }
            catch
            {
                //Application.Current.MainWindow.Dispatcher.BeginInvoke((Action)(() =>
                //{
                //    MessageBox.Show("StreamDeck not connnedted.");
                //}), null);
            }
        }

        private void Deck_KeyPressed(object sender, StreamDeckKeyEventArgs e)
        {
            var d = sender as IStreamDeck;
            if (d == null) return;

            if(Deck == null)
            {
                return;
            }

            if (e.IsDown)
            {
                // Key Down
                MainWindowPanel.Dispatcher.Invoke(() => 
                {
                    Bitmap bmp = (ButtonMatrixPanel.btnList[e.Key].DataContext as ButtonData).ButtonBitmapBuf_DOWN;
                    StreamDeckKeyBitmap sdkb = StreamDeckKeyBitmap.FromDrawingBitmap(bmp);
                    Deck.SetKeyBitmap(e.Key, sdkb);
                    ButtonData bd = ButtonMatrixPanel.btnList[e.Key].DataContext as ButtonData;
                    ButtonFuncProcess(bd);
                });
            }
            else
            {
                // Key up
                MainWindowPanel.Dispatcher.Invoke(() => 
                {
                    Bitmap bmp = (ButtonMatrixPanel.btnList[e.Key].DataContext as ButtonData).ButtonBitmapBuf_UP;
                    StreamDeckKeyBitmap sdkb = StreamDeckKeyBitmap.FromDrawingBitmap(bmp);
                    Deck.SetKeyBitmap(e.Key, sdkb);
                });
            }
        }

        private void ButtonFuncProcess(ButtonData bd)
        {
            switch (bd.FuncType)
            {
                case ButtonType.AHKCommand:
                    ahk.ExecRaw(bd.AHDCmd);
                    break;
                case ButtonType.Goto:
                    if(bd.TargetPage == null)
                    {
                        return;
                    }
                    MainWindowPanel.FindItemToSelect(MainWindowPanel.PageTree, bd.TargetPage.PageKey);
                    break;
                default:
                    break;
            }
        }

        public void RefreshStreamDeckPage()
        {
            if (ButtonMatrixPanel == null || MainWindowPanel == null)
            {
                return;
            }

            for (int i = 0; i < 15; i++)
            {
                Bitmap bmp_up = ButtonMatrixPanel.btnList[i].GetRenderBitmap();
                Bitmap bmp_down = ButtonMatrixPanel.btnList[i].GetRenderBitmap(true);
                StreamDeckKeyBitmap sdkb = StreamDeckKeyBitmap.FromDrawingBitmap(bmp_up);
                if (Deck != null)
                {
                    Deck.SetKeyBitmap(i, sdkb);
                }


                // Save bitmap to ButtonData
                (ButtonMatrixPanel.btnList[i].DataContext as ButtonData).ButtonBitmapBuf_UP = bmp_up;
                (ButtonMatrixPanel.btnList[i].DataContext as ButtonData).ButtonBitmapBuf_DOWN = bmp_down;
            }
        }

        public void RefreshStreamDeckButton(int i)
        {
            if (ButtonMatrixPanel == null || MainWindowPanel == null)
            {
                return;
            }


            Bitmap bmp_up = ButtonMatrixPanel.btnList[i].GetRenderBitmap();
            Bitmap bmp_down = ButtonMatrixPanel.btnList[i].GetRenderBitmap(true);
            StreamDeckKeyBitmap sdkb = StreamDeckKeyBitmap.FromDrawingBitmap(bmp_up);
            if (Deck != null)
            {
                Deck.SetKeyBitmap(i, sdkb);
            }
            
            // Save bitmap to ButtonData
            (ButtonMatrixPanel.btnList[i].DataContext as ButtonData).ButtonBitmapBuf_UP = bmp_up;
            (ButtonMatrixPanel.btnList[i].DataContext as ButtonData).ButtonBitmapBuf_DOWN = bmp_down;
        }

        public void Save(string filename)
        {
            CoreData.GetInstance().SaveToFile(filename);
        }

        public void Load(string filename)
        {
            CoreData.GetInstance().LoadFromFile(filename);
            MainWindowPanel.PageTree.ItemsSource = CoreData.GetInstance().PageTree;
        }

        public void Clear()
        {
            CoreData.GetInstance().Clear();
            MainWindowPanel.PageTree.ItemsSource = CoreData.GetInstance().PageTree;
        }

        public void ClearButton(int Index)
        {

        }

    }

    [Serializable]
    public class CoreData
    {
        private static CoreData m_Instance;

        public static CoreData GetInstance()
        {
            if(m_Instance == null)
            {
                m_Instance = new CoreData();
            }
            return m_Instance;
        }

        private CoreData()
        {
            if(PageTree == null)
            {
                PageTree = new ObservableCollection<PageData>();
                PageData RootPage = new PageData();
                RootPage.PageName = "Root";
                PageTree.Add(RootPage);
                TreeRoot = RootPage;
            }
        }

        public ObservableCollection<PageData> PageTree {get; set;}
        public PageData TreeRoot { get; set; }

        public void SaveToFile(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create,
            FileAccess.Write, FileShare.None);
            formatter.Serialize(stream,m_Instance);
            stream.Close();
        }

        public void LoadFromFile(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open,
            FileAccess.Read, FileShare.Read);
            CoreData obj = (CoreData)formatter.Deserialize(stream);
            stream.Close();

            m_Instance = obj;
        }

        public void Clear()
        {
            m_Instance = new CoreData();
        }

    }
}
