using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace SDC
{
    /// <summary>
    /// Interaction logic for ButtonMatrix.xaml
    /// </summary>
    public partial class ButtonMatrix : UserControl, INotifyPropertyChanged
    {
        public ImgBtn[] btnList = new ImgBtn[15];

        /// <summary>
        /// 选中按钮的前台对象
        /// </summary>
        public ImgBtn SelectedButton
        {
            get
            {
                foreach (ImgBtn i in btnList)
                {
                    if (i.Selected)
                    {
                        return i;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 选中按钮的索引号
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < 15; i++)
                {
                    if (btnList[i].Selected)
                    {
                        return i;
                    }
                }

                return -1;
            }

        }

        /// <summary>
        /// 选中按钮所对应的后台数据对象
        /// </summary>
        public ButtonData SelectedButtonData
        {
            get
            {
                for (int i = 0; i < 15; i++)
                {
                    if (btnList[i].Selected)
                    {
                        return btnList[i].DataContext as ButtonData;
                    }
                }
                return null;
            }
        }

        public ButtonMatrix()
        {
            InitializeComponent();
            btnList[0] = btn1;
            btnList[1] = btn2;
            btnList[2] = btn3;
            btnList[3] = btn4;
            btnList[4] = btn5;
            btnList[5] = btn6;
            btnList[6] = btn7;
            btnList[7] = btn8;
            btnList[8] = btn9;
            btnList[9] = btn10;
            btnList[10] = btn11;
            btnList[11] = btn12;
            btnList[12] = btn13;
            btnList[13] = btn14;
            btnList[14] = btn15;
        }

        public void InitPanel()
        {
            for (int i = 0; i < 15; i++)
            {
                btnList[i].DataContext = (this.DataContext as PageData).BtnList[i];
            }
            NotifyPropertyChange("SelectedButtonData");
            NotifyPropertyChange("SelectedButton");
            NotifyPropertyChange("SelectedIndex");
        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InitPanel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 15; i++)
            {
                btnList[i].MyButtonClick += ButtonMatrix_MyButtonClick;
                btnList[i].Index = i;
            }

            btnList[4].Selected = true;

            NotifyPropertyChange("SelectedButtonData");
            NotifyPropertyChange("SelectedButton");
            NotifyPropertyChange("SelectedIndex");
        }

        private void ButtonMatrix_MyButtonClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // SetFocus
            //(sender as ImgBtn).Selected = true;
            foreach(ImgBtn i in btnList)
            {
                if(i != sender)
                {
                    i.Selected = false;
                }
                else
                {
                    i.Selected = true;
                }
            }

            NotifyPropertyChange("SelectedButtonData");
            NotifyPropertyChange("SelectedButton");
            NotifyPropertyChange("SelectedIndex");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }  
    }
}
