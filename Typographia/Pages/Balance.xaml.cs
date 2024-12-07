using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
    /// Логика взаимодействия для Balance.xaml
    /// </summary>
    public partial class Balance : Page
    {
        string login;
        public Balance(string LOGIN)
        {
            InitializeComponent();
            this.login = LOGIN;
            var tempUser = Class1.dbo.Clients.FirstOrDefault(c => c.Login == login);
            LoadTransactions(login);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtBalanceChange.Text) > 0)
                {
                    var tempUser = Class1.dbo.Clients.FirstOrDefault(c => c.Login == login);
                    var tempTransaction = new Transactions()
                    {
                        Id_client = tempUser.Id_client,
                        TransactionType = "Пополнение",
                        TransactionAmount = Convert.ToInt32(txtBalanceChange.Text),
                    };
                    tempUser.Balance = Convert.ToInt32(txtBalanceChange.Text) + Convert.ToInt32(tempUser.Balance);
                    MainWindow.Duble.NavPanel.NavigationService.Navigate(new NavPanel(tempUser.Login));
                    Class1.dbo.Transactions.Add(tempTransaction);
                    Class1.dbo.SaveChanges();
                    LoadTransactions(login);
                    MessageBox.Show("Пополнение прошло успешно");
                }
                else
                {
                    MessageBox.Show("Сумма пополнения не может быть отрицательной или ровняться нулю");
                }
            }
            catch
            {
                MessageBox.Show("Вы некорректно ввели сумму транзакции");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempUser = Class1.dbo.Clients.FirstOrDefault(c => c.Login == login);
                if (Convert.ToInt32(tempUser.Balance) - Convert.ToInt32(txtBalanceChange.Text) >= 0)
                {
                    if (Convert.ToInt32(txtBalanceChange.Text) > 0)
                    {
                        var tempTransaction = new Transactions()
                        {
                            Id_client = tempUser.Id_client,
                            TransactionType = "Вывод",
                            TransactionAmount = Convert.ToInt32(txtBalanceChange.Text),
                        };
                        tempUser.Balance = Convert.ToInt32(tempUser.Balance) - Convert.ToInt32(txtBalanceChange.Text);
                        MainWindow.Duble.NavPanel.NavigationService.Navigate(new NavPanel(tempUser.Login));
                        Class1.dbo.Transactions.Add(tempTransaction);
                        Class1.dbo.SaveChanges();
                        LoadTransactions(login);
                        MessageBox.Show("Списание прошло успешно");
                    }
                    else
                    {
                        MessageBox.Show("Сумма списания не может быть отрицательной или ровняться нулю");
                    }
                }
                else
                {
                    MessageBox.Show("Недостаточно средств");
                }
            }
            catch
            {
                MessageBox.Show("Вы некорректно ввели сумму транзакции");
            }
        }

        private void LoadTransactions(string login)
        {
            var tempUser = Class1.dbo.Clients.FirstOrDefault(c => c.Login == login);
            if (tempUser == null)
            {
                MessageBox.Show("Пользователь не найден.");
                return;
            }

            string query = @"
SELECT 
    t.Id_transactions AS TransactionId,
    c.FirstName + ' ' + c.LastName AS ClientFullName,
    t.TransactionAmount,
    t.TransactionType
FROM 
    Transactions t
JOIN 
    Clients c ON t.Id_client = c.Id_client
WHERE 
    t.Id_client = @CurrentUserId";

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CurrentUserId", tempUser.Id_client);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var transactionsWithClientInfo = new List<object>();

                    while (reader.Read())
                    {
                        var transaction = new
                        {
                            TransactionId = reader["TransactionId"],
                            ClientFullName = reader["ClientFullName"],
                            TransactionAmount = reader["TransactionAmount"],
                            TransactionType = reader["TransactionType"]
                        };

                        transactionsWithClientInfo.Add(transaction);
                    }
                    Transactions.ItemsSource = transactionsWithClientInfo;
                }
            }
        }
    }
}
