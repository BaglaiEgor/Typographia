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

namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminNav.xaml
    /// </summary>
    public partial class AdminNav : Page
    {
        public AdminNav()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //материалы и продукты
        {
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new Storage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //клиенты и сотрудники
        {
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new ClientsNEmp());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //заказы
        {
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new AdminOrders());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) //уведомления
        {
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new AdminNotification());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) //отчёты
        {
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new Reports());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e) //выход
        {
            MainWindow.Duble.NavPanel.Visibility = Visibility.Hidden;
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new Registration());
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MainWindow.Duble.MainFrame.Navigate(new AdminNotification());
        }
    }
}
