using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SDC
{

    /// <summary>
    /// 按钮页面定义
    /// </summary>
    [Serializable]
    public class PageData : INotifyPropertyChanged
    {
        public ButtonData[] m_BtnList = new ButtonData[15];

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ButtonData[] BtnList
        {
            get { return m_BtnList; }
            set { m_BtnList = value; }
        }

        public ObservableCollection<PageData> SubPages { get; set; }

        public PageData ParentPageData { get; set; }

        private string m_PageName;
        public string PageName
        {
            get
            {
                return m_PageName;
            }
            set
            {
                m_PageName = value;
                NotifyPropertyChange("Pagename");
            }
        }

        public Guid PageKey {get; set;}

        public PageData()
        {
            for(int i=0;i<15;i++)
            {
                m_BtnList[i] = new ButtonData();
            }
            PageName = "Default";
            SubPages = new ObservableCollection<PageData>();
            PageKey = Guid.NewGuid();
        }
    }

    /// <summary>
    /// 按钮定义
    /// </summary>
    [Serializable]
    public class ButtonData: INotifyPropertyChanged
    {
        // Button Facade
        public Bitmap ButtonBitmapBuf_UP { get; set; }
        public Bitmap ButtonBitmapBuf_DOWN { get; set; }

        private SerializableBitmapImageWrapper m_Image;
        public BitmapImage Image
        {
            get
            {
                return m_Image;
            }

            set
            {
                m_Image = value;
                NotifyPropertyChange("Image");
            }
        }

        public byte[] ImageBuffer { get; set; }

        private String m_FontFamilyStr;
        public String FontFamilyStr
        {
            get
            {
                return m_FontFamilyStr;
            }
            set
            {
                m_FontFamilyStr = value;
                NotifyPropertyChange("FontFamilyStr");
            }
        }

        private int m_FontSize;
        public int FontSize
        {
            get
            {
                return m_FontSize;
            }
            set
            {
                m_FontSize = value;
                NotifyPropertyChange("FontSize");
            }
        }

        private String m_FontColor;

        [field: NonSerialized]
        public System.Windows.Media.Color FontColor
        {
            get
            {
                //return System.Windows.Media.Color.from;
                return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(m_FontColor);
            }
            set
            {
                m_FontColor = value.ToString();
                NotifyPropertyChange("FontColor");
            }
        }

        // Common
        private ButtonType m_FuncType;
        public ButtonType FuncType
        {
            get
            {
                return m_FuncType;
            }
            set
            {
                m_FuncType = value;
                NotifyPropertyChange("FuncType");
            }
        }

        private string m_ButtonText;
        public string ButtonText
        {
            get
            {
                return m_ButtonText;
            }
            set
            {
                m_ButtonText = value;
                NotifyPropertyChange("ButtonText");
            }
        }

        private bool m_ShowText;
        public bool ShowText
        {
            get
            {
                return m_ShowText;
            }
            set
            {
                m_ShowText = value;
                NotifyPropertyChange("ShowText");
            }
        }

        private int m_TextTopMargin;
        public int TextTopMargin
        {
            get
            {
                return m_TextTopMargin;
            }
            set
            {
                m_TextTopMargin = value;
                NotifyPropertyChange("TextTopMargin");
            }
        }

        private ButtonPosMode m_ButtonTextPosition;
        public ButtonPosMode ButtonTextPosition
        {
            get
            {
                return m_ButtonTextPosition;
            }
            set
            {
                m_ButtonTextPosition = value;
                NotifyPropertyChange("ButtonTextPosition");
            }
        }

        // Goto: Goto Folder and Return Back
        private PageData m_TargetPage;
        public PageData TargetPage
        {
            get
            {
                return m_TargetPage;
            }
            set
            {
                m_TargetPage = value;
                NotifyPropertyChange("TargetPage");
            }
        }

        // Hotkey
        private string m_AHDCmd;
        public string AHDCmd
        {
            get
            {
                return m_AHDCmd;
            }
            set
            {
                m_AHDCmd = value;
                NotifyPropertyChange("AHDCmd");
            }
        }

        // Command
        private string m_Action;
        public string Action
        {
            get
            {
                return m_Action;
            }
            set
            {
                m_Action = value;
                NotifyPropertyChange("Action");
            }
        }

        public ButtonData()
        {
            Init();
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Init()
        {
            ButtonText = "TEST";
            AHDCmd = "Send #e";
            ButtonTextPosition = ButtonPosMode.Center;
            FuncType = ButtonType.Blank;
            ShowText = false;
            TextTopMargin = 0;
            
            ButtonBitmapBuf_UP = null;
            ButtonBitmapBuf_DOWN = null;
            ImageBuffer = null;
            Image = null;

            FontFamilyStr = "Arial";
            FontSize = 12;
            FontColor = System.Windows.Media.Color.FromRgb(255,255,255);
        }

    }

    public enum ButtonType
    {
        AHKCommand,
        Goto,
        Blank
    };

    public enum ButtonPosMode
    {
        Center,
        Upper,
        Lower,
        Custom
    }

}
