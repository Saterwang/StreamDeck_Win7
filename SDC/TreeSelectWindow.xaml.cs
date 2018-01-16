using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SDC
{
    /// <summary>
    /// Interaction logic for TreeSelectWindow.xaml
    /// </summary>
    public partial class TreeSelectWindow : Window
    {
        public TreeSelectWindow()
        {
            InitializeComponent();
            PageTree.ItemsSource = CoreData.GetInstance().PageTree;
        }

        public PageData SelectedPage { get; set; }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExpandAllItems(PageTree);
            TreeViewDefaultFocus(PageTree);
        }

        private void PageTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedPage = e.NewValue as PageData;
        }
    }
}
