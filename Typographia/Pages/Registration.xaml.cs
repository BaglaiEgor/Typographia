using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Xml.Linq;
using Typographia.db;
using Typographia.Pages;
using System.Drawing;
using static QRCoder.PayloadGenerator;
using System.IO;

namespace Typographia
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private Random random;
        public Registration()
        {
            InitializeComponent();
        }
        private bool IsValidPhoneNumber(string number)
        {
            string pattern = @"^\+\d{1,12}$";
            return Regex.IsMatch(number, pattern);
        }
        string qwe;
        private void GenerateEmailQRCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 10000);
            qwe = randomNumber.ToString();
            string emailBody = $"Ваш код: {randomNumber}";
            string emailAddress = "egorbaglai228@gmail.com";
            string emailLink = $"mailto:{emailAddress}?subject=Код&body={Uri.EscapeDataString(emailBody)}";
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(emailLink, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrImage = qrCode.GetGraphic(20))
                    {
                        qrCodeImage.Source = ConvertBitmapToBitmapImage(qrImage);
                    }
                }
            }
        }
        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
        private void Login_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(txtLogin.Text == "AdminDarkWillow")
            {
                code.Visibility = Visibility.Visible;
                qrCodeImage.Visibility = Visibility.Visible;
                admin.Visibility = Visibility.Visible;
                GenerateEmailQRCode();
            }
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtContactDetails.Text != "" && chkAgree.IsChecked ==true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }
        private void txtPassword_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtContactDetails.Text != "" && chkAgree.IsChecked == true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }

        private void txtContactDetails_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtContactDetails.Text != "" && chkAgree.IsChecked == true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }

        private void chkAgree_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtContactDetails.Text != "" && chkAgree.IsChecked == true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }

        private void txtUsLogin_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtUsLogin.Text != "" && txtUsPassword.Text != "")
            {
                buttLog.IsEnabled = true;
            }
            else
            {
                buttLog.IsEnabled = false;
            }
        }

        private void txtUsPassword_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtUsLogin.Text != "" && txtUsPassword.Text != "")
            {
                buttLog.IsEnabled = true;
            }
            else
            {
                buttLog.IsEnabled = false;
            }
        }

        private void buttReg_Click(object sender, RoutedEventArgs e)
        {
            string LoginUser = txtLogin.Text;
            string ContactDetails = txtContactDetails.Text;
            bool isValid = IsValidPhoneNumber(txtContactDetails.Text);
            var User = Class1.dbo.Clients.FirstOrDefault(name => name.Login == LoginUser);
            var Emp = Class1.dbo.Employee.FirstOrDefault(emp => emp.Login == LoginUser);
            var UsCt = Class1.dbo.Clients.FirstOrDefault(uct => uct.ContactDetails == ContactDetails);
            var EmCt = Class1.dbo.Employee.FirstOrDefault(ect => ect.ContactDetails == ContactDetails);
            if (User != null || Emp != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }
            if (UsCt != null || EmCt != null)
            {
                MessageBox.Show("Пользователь с таким номером телефона уже существует!");
                return;
            }
            if (!isValid)
            {
                MessageBox.Show("Телефон имеет неверный формат");
                return;
            }
            var tempClients = new db.Clients()
                {
                    Login = txtLogin.Text,
                    Password = txtPassword.Text,
                    FirstName = txtFName.Text,
                    LastName = txtLName.Text,
                    ContactDetails = txtContactDetails.Text,
                    Preferences = txtPreferences.Text,
                    Balance = 0
                };
                Class1.dbo.Clients.Add(tempClients);
                Class1.dbo.SaveChanges();
                MessageBox.Show("Клиент успешно зарегестрирован!");
        }

        private void buttLog_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == "EmployeeDarkWillow")
            {
                string LoginEmp = txtUsLogin.Text;
                string PassEmp = txtUsPassword.Text;
                var Passw = Class1.dbo.Employee.FirstOrDefault(pass => pass.Password == PassEmp);
                var Emp = Class1.dbo.Employee.FirstOrDefault(name => name.Login == LoginEmp);
                var Tele = Class1.dbo.Employee.FirstOrDefault(tel => tel.ContactDetails == LoginEmp);
                if (Emp == null)
                {
                    Emp = Class1.dbo.Employee.FirstOrDefault(tel => tel.ContactDetails == LoginEmp);
                }
                if (Emp == null)
                {
                    MessageBox.Show("Сотрудник с таким логином или номером телефона не зарегистрирован!");
                    return;
                }
                else if (Passw == null)
                {
                    MessageBox.Show("Неверный пароль");
                    return;
                }
                else
                {
                    MainWindow.Duble.MainFrame.Navigate(new EmpMenu(Emp.Login));
                    MainWindow.Duble.NavPanel.Visibility = Visibility.Visible;
                    MainWindow.Duble.NavPanel.Navigate(new EmpPanel(Emp.Login));
                    return;
                }
            }
            string LoginUser = txtUsLogin.Text;
            string PassUser = txtUsPassword.Text;
            var Pass = Class1.dbo.Clients.FirstOrDefault(pass => pass.Password == PassUser);
            var User = Class1.dbo.Clients.FirstOrDefault(name => name.Login == LoginUser);
            var Tel = Class1.dbo.Clients.FirstOrDefault(tel => tel.ContactDetails == LoginUser);
            if (User == null)
            {
                User = Class1.dbo.Clients.FirstOrDefault(tel => tel.ContactDetails == LoginUser);
            }
            if (User == null)
            {
                MessageBox.Show("Клиент с таким логином или номером телефона не зарегистрирован!");
            }
            else if (Pass == null)
            {
                MessageBox.Show("Неверный пароль");
            }
            else
            {
                MainWindow.Duble.MainFrame.Navigate(new Profile(User.Login));
                MainWindow.Duble.NavPanel.Visibility = Visibility.Visible;
                MainWindow.Duble.NavPanel.Navigate(new NavPanel(User.Login));
            }
        }

        private void code_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (code.Text == qwe)
                {
                    admin.IsEnabled = true;
                }
                else
                {
                    admin.IsEnabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Некорректно заполнено поле");
            }
        }

        private void admin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Duble.NavPanel.Navigate(new AdminNav());
            MainWindow.Duble.NavPanel.Visibility = Visibility.Visible;
            MainWindow.Duble.MainFrame.NavigationService.Navigate(new ClientsNEmp());
        }
    }
}
