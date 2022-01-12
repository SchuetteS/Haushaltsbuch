using FinanzenUI_WPF.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanzenUI_WPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Page> openPages;
        public MainWindow()
        {
            InitializeComponent();

            openPages = new List<Page>();
            openPages.Add(new Dashboard());         // Page 0
            openPages.Add(new Import());            // Page 1
            openPages.Add(new Kontobewegungen());   // Page 2

            frame.Content = openPages[0];
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void btnStartseite_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = openPages[0];
        }

        private void btnImport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = openPages[1];
        }

        private void btnMoneyTransfer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = openPages[2];
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (tgMenu.IsChecked == true)
            {
                //tt_menu.Visibility = Visibility.Collapsed;
                tt_Startseite.Visibility = Visibility.Collapsed;
                tt_MoneyTransfer.Visibility = Visibility.Collapsed;
                tt_Import.Visibility = Visibility.Collapsed;
                tt_Settings.Visibility = Visibility.Collapsed;
            }
            else
            {
                //tt_menu.Visibility = Visibility.Visible;
                tt_Startseite.Visibility = Visibility.Visible;
                tt_MoneyTransfer.Visibility = Visibility.Visible;
                tt_Import.Visibility = Visibility.Visible;
                tt_Settings.Visibility = Visibility.Visible;
            }
        }
    }
}
