using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Conf_Cmd.xaml
    /// </summary>
    public partial class Conf_Cmd : UserControl
    {
        public Conf_Cmd()
        {
            InitializeComponent();
        }

        private void posMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            switch ((ButtonPosMode)e.AddedItems[0])
            {
                case ButtonPosMode.Custom:
                    {
                        topMargin.IsEnabled = true;
                    }
                    break;
                default:
                    {
                        topMargin.IsEnabled = false;
                    }
                    break;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            posMode.ItemsSource = Enum.GetValues(typeof(ButtonPosMode)).Cast<ButtonPosMode>();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
