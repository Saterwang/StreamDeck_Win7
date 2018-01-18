using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SDC
{
    /// <summary>
    /// BtnPropertyPanel.xaml 的交互逻辑
    /// </summary>
    public partial class BtnPropertyPanel : System.Windows.Controls.UserControl
    {
        public BtnPropertyPanel()
        {
            InitializeComponent();
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Set default combo box value
            FuncSelect.ItemsSource = Enum.GetValues(typeof(ButtonType)).Cast<ButtonType>();
            autoStart.IsChecked = AutoStartup.Check();
        }

        private void FuncSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count <= 0)
            {
                return;
            }
            ButtonType currentConfPanel = (ButtonType)e.AddedItems[0];
            ConfPanel.Children.Clear();
            switch (currentConfPanel)
            {
                case ButtonType.AHKCommand:
                    {
                        Conf_Autokey panel = new Conf_Autokey();
                        ConfPanel.Children.Add(panel);
                    }
                    break;
                case ButtonType.Goto:
                    {
                        Conf_Goto panel = new Conf_Goto();
                        ConfPanel.Children.Add(panel);
                    }
                    break;
                case ButtonType.Blank:
                    {
                        Business.GetInstance().ButtonMatrixPanel.SelectedButtonData.Init();
                    }
                    break;
                default:
                    break;
            }
        }

        private void btn_MainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.contextMenu.PlacementTarget = this.btn_MainMenu;
            //位置  
            this.contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            //显示菜单  
            this.contextMenu.IsOpen = true; 
        }

        private void savePage_Click(object sender, RoutedEventArgs e)
        {
            string s = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf('\\')) + "\\Config.stm";
            Business.GetInstance().Save(s);
        }

        private void loadPage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = "StreamDeck Config Files|*.stm";
            dlg.Title = "Select a StreamDeck Config File";
            dlg.InitialDirectory = System.Windows.Forms.Application.StartupPath;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Business.GetInstance().Load(dlg.FileName);
            }
        }

        private void btn_MainMenu_Initialized(object sender, EventArgs e)
        {
            this.btn_MainMenu.ContextMenu = null;   
        }

        private void saveAsPage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.Filter = "StreamDeck Config Files|*.stm";
            dlg.Title = "Select a StreamDeck Config File";
            dlg.InitialDirectory = System.Windows.Forms.Application.StartupPath;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Business.GetInstance().Save(dlg.FileName);
            }
        }

        private void autoStart_Checked(object sender, RoutedEventArgs e)
        {
            AutoStartup.Set(true);
        }

        private void autoStart_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoStartup.Set(false);
        }

        private void clearPage_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Confirm to clear all the data?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Business.GetInstance().Clear();
            }
        }
    }
}
