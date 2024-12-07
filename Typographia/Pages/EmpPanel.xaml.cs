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
using Typographia.db;

namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmpPanel.xaml
    /// </summary>
    public partial class EmpPanel : Page
    {
        string _login;
        public EmpPanel(string login)
        {
            InitializeComponent();
            _login = login;
            var tempUser = Class1.dbo.Employee.FirstOrDefault(c => c.Login == login);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new EmpMenu(_login));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Duble.MainFrame.Navigate(new EmpOrder(_login));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.Duble.MainFrame.Navigate(new EmpNotification(_login));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Duble.NavPanel.Visibility = Visibility.Hidden;
            MainWindow.Duble.MainFrame.Navigate(new Registration());
        }
    }
}
