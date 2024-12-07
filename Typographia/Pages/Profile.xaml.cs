using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Typographia.Pages;

namespace Typographia
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Profile : Page
    {
        string login;
        public Profile(string LOGIN)
        {
            InitializeComponent();
            this.login = LOGIN;
            var tempUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);
            {
                txtUserName.Text = tempUser.Login;
                txtLogin.Text = tempUser.Login;
                txtPassword.Text = tempUser.Password;
                txtFName.Text = tempUser.FirstName;
                txtLName.Text = tempUser.LastName;
                txtContactDetails.Text = tempUser.ContactDetails;
                txtPreferences.Text = tempUser.Preferences;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EditCB.SelectedItem is ComboBoxItem item)
            {
                if (item.Content.ToString() == "Логин")
                {
                    var currentUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);
                    var existingUserWithNewLogin = Class1.dbo.Clients
                    .FirstOrDefault(user => user.Login == txtChanged.Text && user.Login != currentUser.Login);

                    if (existingUserWithNewLogin != null)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует. Пожалуйста, выберите другой логин.");
                    }
                    else if (existingUserWithNewLogin == null)
                    {
                        currentUser.Login = txtChanged.Text;
                        txtUserName.Text = currentUser.Login;
                        txtLogin.Text = currentUser.Login;
                        Class1.dbo.SaveChanges();
                        MainWindow.Duble.NavPanel.NavigationService.Navigate(new NavPanel(currentUser.Login));
                    }
                }
                else if (item.Content.ToString() == "Пароль")
                {
                    var tempUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);
                    tempUser.Password = txtChanged.Text;
                    txtPassword.Text = tempUser.Password;
                    Class1.dbo.SaveChanges();
                }
                else if (item.Content.ToString() == "Имя")
                {
                    var tempUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);
                    tempUser.FirstName = txtChanged.Text;
                    txtFName.Text = tempUser.FirstName;
                    Class1.dbo.SaveChanges();
                }
                else if (item.Content.ToString() == "Фамилия")
                {
                    var tempUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);
                    tempUser.LastName = txtChanged.Text;
                    txtLName.Text = tempUser.LastName;
                    Class1.dbo.SaveChanges();
                }
                else if (item.Content.ToString() == "Номер телефона")
                {
                    string login = txtLogin.Text;
                    var currentUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);

                    if (currentUser == null)
                    {
                        MessageBox.Show("Текущий пользователь не найден.");
                    }
                    var existingUserWithNewPhone = Class1.dbo.Clients
                        .FirstOrDefault(user => user.ContactDetails == txtChanged.Text && user.Id_client != currentUser.Id_client);

                    if (existingUserWithNewPhone != null)
                    {
                        MessageBox.Show("Пользователь с таким номером уже существует. Пожалуйста, выберите другой номер.");
                    }
                    else
                    {
                        currentUser.ContactDetails = txtChanged.Text;
                        txtContactDetails.Text = currentUser.ContactDetails;
                        Class1.dbo.SaveChanges();
                        MainWindow.Duble.NavPanel.NavigationService.Navigate(new NavPanel(currentUser.Login));
                    }
                }
                else if(item.Content.ToString() == "Предпочтения")
                {
                    var tempUser = Class1.dbo.Clients.FirstOrDefault(user => user.Login == login);
                    tempUser.Preferences = txtChanged.Text;
                    txtPreferences.Text = tempUser.Preferences;
                    Class1.dbo.SaveChanges();
                }
                else if (txtChanged.Text == "")
                {
                    MessageBox.Show("Вы ввели значение, на которое хотите поменять");
                }
                else
                {
                    MessageBox.Show("Вы не выбрали, что хотите менять");
                }
            }
        }
    }
}
