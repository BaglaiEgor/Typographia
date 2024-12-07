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
    /// Логика взаимодействия для EmpMenu.xaml
    /// </summary>
    public partial class EmpMenu : Page
    {
        string login;
        public EmpMenu(string LOGIN)
        {
            InitializeComponent();
            this.login = LOGIN;
            var tempUser = Class1.dbo.Employee.FirstOrDefault(user => user.Login == login);
            {
                txtUserName.Text = tempUser.Login;
                txtLogin.Text = tempUser.Login;
                txtPassword.Text = tempUser.Password;
                txtFName.Text = tempUser.FirstName;
                txtLName.Text = tempUser.LastName;
                txtContactDetails.Text = tempUser.ContactDetails;
            }
        }
    }
}
