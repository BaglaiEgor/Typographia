using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
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
using Typographia.db;


namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsNEmp.xaml
    /// </summary>
    public partial class ClientsNEmp : Page
    {
        public ClientsNEmp()
        {
            InitializeComponent();
            LoadDataClients();
            LoadDataEmp();
        }
        private void LoadDataClients()
        {
            string query = @"SELECT Id_client, Login, Password, FirstName, LastName, ContactDetails, Preferences, Balance FROM Clients";

            List<Clients> clientsList = new List<Clients>();

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var client = new Clients
                        {
                            Id_client = reader.GetInt32(0),
                            Login = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Password = reader.IsDBNull(2) ? null : reader.GetString(2),
                            FirstName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            LastName = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ContactDetails = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Preferences = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Balance = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7)
                        };

                        clientsList.Add(client);
                    }
                }
            }

            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.Items.Clear();
            ClientsDataGrid.ItemsSource = clientsList;
        }
        private void LoadDataEmp()
        {
            string query = @"SELECT Id_employee, Login, Password, FirstName, LastName, ContactDetails FROM Employee";

            List<Employee> empList = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var emp = new Employee
                        {
                            Id_employee = reader.GetInt32(0),
                            Login = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Password = reader.IsDBNull(2) ? null : reader.GetString(2),
                            FirstName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            LastName = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ContactDetails = reader.IsDBNull(5) ? null : reader.GetString(5),
                        };

                        empList.Add(emp);
                    }
                }
            }
            EmpDataGrid.ItemsSource = null;
            EmpDataGrid.Items.Clear();
            EmpDataGrid.ItemsSource = empList;
        }

        private void ClientsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Clients selectedClient = (Clients)ClientsDataGrid.SelectedItem;

            if (selectedClient != null)
            {
                var editedCell = e.EditingElement as TextBox;
                if (editedCell != null)
                {
                    string newValue = editedCell.Text;

                    switch (e.Column.Header.ToString())
                    {
                        case "Login":
                            selectedClient.Login = newValue;
                            break;
                        case "Password":
                            selectedClient.Password = newValue;
                            break;
                        case "First Name":
                            selectedClient.FirstName = newValue;
                            break;
                        case "Last Name":
                            selectedClient.LastName = newValue;
                            break;
                        case "Contact Details":
                            selectedClient.ContactDetails = newValue;
                            break;
                        case "Preferences":
                            selectedClient.Preferences = newValue;
                            break;
                        case "Balance":
                            if (decimal.TryParse(newValue, out decimal balance))
                            {
                                selectedClient.Balance = balance;
                            }
                            break;
                    }
                    try
                    {
                        SaveClientChanges(selectedClient);
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SaveClientChanges(Clients client)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
                {
                    string checkLoginQuery = @"
                SELECT COUNT(*) 
                FROM (
                    SELECT Login FROM Clients WHERE Login = @Login AND Id_client <> @Id_client
                    UNION ALL
                    SELECT Login FROM Employee WHERE Login = @Login
                ) AS CombinedResults";

                    using (SqlCommand checkLoginCommand = new SqlCommand(checkLoginQuery, connection))
                    {
                        checkLoginCommand.Parameters.AddWithValue("@Login", client.Login);
                        checkLoginCommand.Parameters.AddWithValue("@Id_client", client.Id_client);
                        connection.Open();
                        int loginCount = (int)checkLoginCommand.ExecuteScalar();

                        if (loginCount > 0)
                        {
                            MessageBox.Show("Логин должен быть уникальным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    string checkContactDetailsQuery = @"
                SELECT COUNT(*) 
                FROM (
                    SELECT ContactDetails FROM Clients WHERE ContactDetails = @ContactDetails AND Id_client <> @Id_client
                    UNION ALL
                    SELECT ContactDetails FROM Employee WHERE ContactDetails = @ContactDetails
                ) AS CombinedResults";

                    using (SqlCommand checkContactDetailsCommand = new SqlCommand(checkContactDetailsQuery, connection))
                    {
                        checkContactDetailsCommand.Parameters.AddWithValue("@ContactDetails", client.ContactDetails);
                        checkContactDetailsCommand.Parameters.AddWithValue("@Id_client", client.Id_client);

                        int contactDetailsCount = (int)checkContactDetailsCommand.ExecuteScalar();
                        if (contactDetailsCount > 0)
                        {
                            MessageBox.Show("Номер телефона должен быть уникальным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                string query = @"UPDATE Clients SET 
                Login = @Login, 
                Password = @Password, 
                FirstName = @FirstName, 
                LastName = @LastName, 
                ContactDetails = @ContactDetails, 
                Preferences = @Preferences, 
                Balance = @Balance 
            WHERE Id_client = @Id_client";

                using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Login", client.Login);
                    command.Parameters.AddWithValue("@Password", client.Password);
                    command.Parameters.AddWithValue("@FirstName", client.FirstName);
                    command.Parameters.AddWithValue("@LastName", client.LastName);
                    command.Parameters.AddWithValue("@ContactDetails", client.ContactDetails);
                    command.Parameters.AddWithValue("@Preferences", client.Preferences);
                    command.Parameters.AddWithValue("@Balance", client.Balance);
                    command.Parameters.AddWithValue("@Id_client", client.Id_client);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsValidPhoneNumber(string number)
        {
            string pattern = @"^\+\d{1,12}$";
            return Regex.IsMatch(number, pattern);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clients selectedClient = ClientsDataGrid.SelectedItem as Clients;
            if (selectedClient != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {selectedClient.FirstName} {selectedClient.LastName}?",
                                                          "Подтверждение удаления",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var clientToDelete = Class1.dbo.Clients.Find(selectedClient.Id_client);
                        if (clientToDelete != null)
                        {
                            Class1.dbo.Clients.Remove(clientToDelete);
                            Class1.dbo.SaveChanges();
                            LoadDataClients();
                        }
                        else
                        {
                            MessageBox.Show("Клиент не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string LoginUser = txtLogin.Text.Trim();
            string ContactDetails = txtContactDetails.Text.Trim();
            bool isValid = IsValidPhoneNumber(ContactDetails);
            var User = Class1.dbo.Clients.FirstOrDefault(name => name.Login == LoginUser);
            var Emp = Class1.dbo.Employee.FirstOrDefault(emp => emp.Login == LoginUser);
            var UsCt = Class1.dbo.Clients.FirstOrDefault(uct => uct.ContactDetails == ContactDetails);
            var EmCt = Class1.dbo.Employee.FirstOrDefault(ect => ect.ContactDetails == ContactDetails);
            if (string.IsNullOrWhiteSpace(LoginUser) || string.IsNullOrWhiteSpace(ContactDetails) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
                Login = LoginUser,
                Password = txtPassword.Text,
                FirstName = txtFName.Text,
                LastName = txtLName.Text,
                ContactDetails = ContactDetails,
                Preferences = txtPreferences.Text,
                Balance = 0
            };
            txtLogin.Clear();
            txtPassword.Clear();
            txtFName.Clear();
            txtLName.Clear();
            txtContactDetails.Clear();
            txtPreferences.Clear();
            Class1.dbo.Clients.Add(tempClients);
            Class1.dbo.SaveChanges();
            LoadDataClients();
            MessageBox.Show("Клиент успешно зарегистрирован!");
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string LoginUser = txtLogin_Копировать.Text.Trim();
            string ContactDetails = txtContactDetails_Копировать.Text.Trim();
            bool isValid = IsValidPhoneNumber(ContactDetails);
            var User = Class1.dbo.Clients.FirstOrDefault(name => name.Login == LoginUser);
            var Emp = Class1.dbo.Employee.FirstOrDefault(emp => emp.Login == LoginUser);
            var UsCt = Class1.dbo.Clients.FirstOrDefault(uct => uct.ContactDetails == ContactDetails);
            var EmCt = Class1.dbo.Employee.FirstOrDefault(ect => ect.ContactDetails == ContactDetails);
            if (string.IsNullOrWhiteSpace(LoginUser) || string.IsNullOrWhiteSpace(ContactDetails) ||
                string.IsNullOrWhiteSpace(txtPassword_Копировать.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            var tempEmp = new db.Employee()
                {
                    Login = txtLogin_Копировать.Text,
                    Password = txtPassword_Копировать.Text,
                    FirstName = txtFName_Копировать.Text,
                    LastName = txtLName_Копировать.Text,
                    ContactDetails = txtContactDetails_Копировать.Text
                };
                txtLogin_Копировать.Clear();
                txtPassword_Копировать.Clear();
                txtFName_Копировать.Clear();
                txtLName_Копировать.Clear();
                txtContactDetails_Копировать.Clear();
                Class1.dbo.Employee.Add(tempEmp);
                Class1.dbo.SaveChanges();
                LoadDataEmp();
                MessageBox.Show("Сотрудник успешно зарегестрирован!");
        }
        private void EmpDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Employee selectedClient = (Employee)EmpDataGrid.SelectedItem;

            if (selectedClient != null)
            {
                var editedCell = e.EditingElement as TextBox;
                if (editedCell != null)
                {
                    string newValue = editedCell.Text;

                    switch (e.Column.Header.ToString())
                    {
                        case "Login":
                            selectedClient.Login = newValue;
                            break;
                        case "Password":
                            selectedClient.Password = newValue;
                            break;
                        case "First Name":
                            selectedClient.FirstName = newValue;
                            break;
                        case "Last Name":
                            selectedClient.LastName = newValue;
                            break;
                        case "Contact Details":
                            selectedClient.ContactDetails = newValue;
                            break;
                    }
                    SaveEmpChanges(selectedClient);
                }
            }
        }
        private void SaveEmpChanges(Employee emp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
                {
                    string checkLoginQuery = @"
                SELECT COUNT(*) 
                FROM (
                    SELECT Login FROM Clients WHERE Login = @Login
                    UNION ALL
                    SELECT Login FROM Employee WHERE Login = @Login AND Id_employee <> @Id_employee
                ) AS CombinedResults";

                    using (SqlCommand checkLoginCommand = new SqlCommand(checkLoginQuery, connection))
                    {
                        checkLoginCommand.Parameters.AddWithValue("@Login", emp.Login);
                        checkLoginCommand.Parameters.AddWithValue("@Id_employee", emp.Id_employee);
                        connection.Open();
                        int loginCount = (int)checkLoginCommand.ExecuteScalar();

                        if (loginCount > 0)
                        {
                            MessageBox.Show("Логин должен быть уникальным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    string checkContactDetailsQuery = @"
                SELECT COUNT(*) 
                FROM (
                    SELECT ContactDetails FROM Clients WHERE ContactDetails = @ContactDetails
                    UNION ALL
                    SELECT ContactDetails FROM Employee WHERE ContactDetails = @ContactDetails AND Id_employee <> @Id_employee
                ) AS CombinedResults";

                    using (SqlCommand checkContactDetailsCommand = new SqlCommand(checkContactDetailsQuery, connection))
                    {
                        checkContactDetailsCommand.Parameters.AddWithValue("@ContactDetails", emp.ContactDetails);
                        checkContactDetailsCommand.Parameters.AddWithValue("@Id_client", emp.Id_employee);

                        int contactDetailsCount = (int)checkContactDetailsCommand.ExecuteScalar();
                        if (contactDetailsCount > 0)
                        {
                            MessageBox.Show("Номер телефона должен быть уникальным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                string query = @"UPDATE Clients SET 
                Login = @Login, 
                Password = @Password, 
                FirstName = @FirstName, 
                LastName = @LastName, 
                ContactDetails = @ContactDetails
            WHERE Id_employee = @Id_employee";

                using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Login", emp.Login);
                    command.Parameters.AddWithValue("@Password", emp.Password);
                    command.Parameters.AddWithValue("@FirstName", emp.FirstName);
                    command.Parameters.AddWithValue("@LastName", emp.LastName);
                    command.Parameters.AddWithValue("@ContactDetails", emp.ContactDetails);
                    command.Parameters.AddWithValue("@Id_employee", emp.Id_employee);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Employee selectedClient = EmpDataGrid.SelectedItem as Employee;
            if (selectedClient != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {selectedClient.FirstName} {selectedClient.LastName}?",
                                                          "Подтверждение удаления",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var empToDelete = Class1.dbo.Employee.Find(selectedClient.Id_employee);
                        if (empToDelete != null)
                        {
                            Class1.dbo.Employee.Remove(empToDelete);
                            Class1.dbo.SaveChanges();
                            LoadDataEmp();
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
