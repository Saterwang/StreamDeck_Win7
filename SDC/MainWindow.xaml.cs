using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.VisualBasic;
using StreamDeckSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace SDC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Business.GetInstance().ButtonMatrixPanel = btnMartix;
            Business.GetInstance().MainWindowPanel = this;
        }

        private static void ExpandAllItems(ItemsControl control)
        {
            if (control == null)
            {
                return;
            }

            foreach (Object item in control.Items)
            {
                System.Windows.Controls.TreeViewItem treeItem = control.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                if (treeItem == null || !treeItem.HasItems)
                {
                    continue;
                }

                treeItem.IsExpanded = true;
                ExpandAllItems(treeItem as ItemsControl);
            }
        }
        public void TreeViewDefaultFocus(ItemsControl control)
        {
            foreach (Object oit in control.Items)
            {
                System.Windows.Controls.TreeViewItem tvi = control.ItemContainerGenerator.ContainerFromItem(oit) as TreeViewItem;
                tvi.IsExpanded = true;
                tvi.Focus();
                break;
            }
        }

        public bool FindItemToSelect(ItemsControl ctrl, Guid pageKey)
        {
            foreach (object obj in ctrl.Items)
            {
                TreeViewItem item = (TreeViewItem)ctrl.ItemContainerGenerator.ContainerFromItem(obj);

                // If the TreeViewItem matches the Frame URI, select the item.
                if (pageKey == (obj as PageData).PageKey)
                {
                    if (item != null && !item.IsSelected)
                        item.IsSelected = true;
                    return true;
                }
                // Expand the item to search nested items.
                if (item != null)
                {
                    bool isExpanded = item.IsExpanded;
                    item.IsExpanded = true;
                    if (item.HasItems && FindItemToSelect(item, pageKey))
                        return true;
                    item.IsExpanded = isExpanded;
                }
            }
            return false;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string pName = Interaction.InputBox("Please Enter Page Name");
            if (pName == "" || pName == string.Empty)
            {
                return;
            }

            var p = new PageData() { PageName = pName };
            //CoreData.GetInstance()..SubPages.Add(p);
            // 找到当前选中的节点
            PageData currPage = PageTree.SelectedItem as PageData;

            p.ParentPageData = currPage;

            // 在当前节点下新增一个节点
            currPage.SubPages.Add(p);

            // 选中新加的节点
            FindItemToSelect(PageTree, p.PageKey);
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            // 找到当前选中的节点
            PageData currPage = PageTree.SelectedItem as PageData;
            currPage.ParentPageData.SubPages.Remove(currPage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PageTree.ItemsSource = CoreData.GetInstance().PageTree;

            if (Business.GetInstance().Deck != null)
            {
                Business.GetInstance().Deck.SetBrightness(100);
                Business.GetInstance().Deck.ClearKeys();
            }

            // Load default config
            string s = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf('\\')) + "\\Config.stm";
            if (File.Exists(s))
            {
                try
                {
                    Business.GetInstance().Load(s);
                }
                catch
                { } 
            }

            InitialTray(); //最小化至托盘

            // Expend Tree Items
            ExpandAllItems(PageTree);
            TreeViewDefaultFocus(PageTree);

            //throw new ArgumentNullException("这是故意抛出的异常");
        }

        private void PageTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 在树的当前选项改变时，更新按钮矩阵
            PageData currPage = e.NewValue as PageData;
            btnMartix.DataContext = currPage;
        }

        private NotifyIcon _notifyIcon = new NotifyIcon();

        #region 最小化系统托盘
        private void InitialTray()
        {
            string s = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf('\\')) + "\\SysIcon.ico";
            //隐藏主窗体
            this.Visibility = Visibility.Hidden;
            //设置托盘的各个属性
 
            _notifyIcon.BalloonTipText = "StreamDeck Running...";//托盘气泡显示内容
            _notifyIcon.Text = "StreamDeck";
            _notifyIcon.Visible = true;//托盘按钮是否可见
            _notifyIcon.Icon = new Icon(s);//托盘中显示的图标
            _notifyIcon.ShowBalloonTip(1000);//托盘气泡显示时间
            _notifyIcon.MouseClick += _notifyIcon_MouseClick;
            //窗体状态改变时触发
            this.StateChanged += MainWindow_StateChanged;
        }

        private void _notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }
  
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Business.GetInstance().Deck == null)
            {
                return;
            }
            Business.GetInstance().Deck.ClearKeys();
            Business.GetInstance().Deck.ShowLogo();
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            PageData currPage = PageTree.SelectedItem as PageData;
            string pName = Interaction.InputBox("Please Enter Page Name", "Rename", currPage.PageName);
            if (pName == "" || pName == string.Empty)
            {
                return;
            }
            currPage.PageName = pName;
        }
    }
}
