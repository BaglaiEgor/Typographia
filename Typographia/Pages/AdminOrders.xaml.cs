using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminOrders.xaml
    /// </summary>
    public partial class AdminOrders : Page
    {
        public AdminOrders()
        {
            InitializeComponent();
            LoadData();
            OrdersDetailsDataGrid.ItemsSource = null;
            OrdersDetailsDataGrid.Items.Clear();
            OrdersDetailsDataGrid.ItemsSource = Class1.dbo.Order_details.ToList();

        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProperties();
        }

        private void FilterProperties()
        {
            int? selectedStatusID = StatusComboBox.SelectedValue as int?;

            List<Orders> orders = LoadAllOrders();
            List<Orders> filteredOrders = new List<Orders>();

            foreach (var order in orders)
            {
                bool matchesStatus = !selectedStatusID.HasValue || order.Id_status == selectedStatusID.Value;

                if (matchesStatus)
                {
                    filteredOrders.Add(order);
                }
            }

            OrdersDataGrid.ItemsSource = filteredOrders;
        }

        private List<Orders> LoadAllOrders()
        {
            List<Orders> orders = new List<Orders>();
            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                connection.Open();
                string query = "SELECT Id_orders, Date, Id_clients, Id_employee, Id_stages, Id_status, Total_cost FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Orders
                            {
                                Id_orders = reader["Id_orders"] != DBNull.Value ? Convert.ToInt32(reader["Id_orders"]) : 0,
                                Date = reader["Date"] != DBNull.Value ? Convert.ToDateTime(reader["Date"]) : DateTime.MinValue,
                                Id_clients = reader["Id_clients"] != DBNull.Value ? Convert.ToInt32(reader["Id_clients"]) : 0,
                                Id_employee = reader["Id_employee"] != DBNull.Value ? Convert.ToInt32(reader["Id_employee"]) : 0,
                                Id_stages = reader["Id_stages"] != DBNull.Value ? Convert.ToInt32(reader["Id_stages"]) : 0,
                                Id_status = reader["Id_status"] != DBNull.Value ? Convert.ToInt32(reader["Id_status"]) : 0,
                                Total_cost = reader["Total_cost"] != DBNull.Value ? Convert.ToDecimal(reader["Total_cost"]) : 0m
                            });
                        }
                    }
                }
            }
            return orders;
        }

            private List<Orders> allorders;

        private void LoadData()
        {
            allorders = LoadAllOrders();

            List<Status> Statuses = LoadStatuses();
            var products = Class1.dbo.Product.ToList(); 
            ProductComboBox.ItemsSource = products.Select(p => p.Name).ToList();
            StatusComboBox.ItemsSource = Statuses;
            StatusComboBox.DisplayMemberPath = "Name";
            StatusComboBox.SelectedValuePath = "Id_status";
            OrdersDataGrid.ItemsSource = null;
            OrdersDataGrid.Items.Clear();
            OrdersDataGrid.ItemsSource = allorders;
        }
        private List<Status> LoadStatuses()
        {
            List<Status> status = new List<Status>();
            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                connection.Open();
                string query = "SELECT Id_status, Name FROM Status";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status.Add(new Status
                            {
                                Id_status = (int)reader["Id_status"],
                                Name = (string)reader["Name"]
                            });
                        }
                    }
                }
            }
            return status;
        }


            private void filterReset_Click(object sender, RoutedEventArgs e)
            {
                StatusComboBox.SelectedIndex = -1;
                allorders = LoadAllOrders();
                OrdersDataGrid.ItemsSource = allorders;
            }

        private void buttNext_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {
                try
                {
                    var selectedOrder = OrdersDataGrid.SelectedItem as Orders;
                    var tempOrders = Class1.dbo.Orders.FirstOrDefault(o => o.Id_orders==selectedOrder.Id_orders);
                    if (EditComboBox.Text == "Stage")
                    {
                        List<Stages> stages = Class1.dbo.Stages.ToList();
                        int currentStageIndex = stages.FindIndex(s => s.Id_stages == selectedOrder.Id_stages);

                        if (currentStageIndex >= 0 && currentStageIndex < stages.Count - 1)
                        {
                            selectedOrder.Id_stages = stages[currentStageIndex + 1].Id_stages;
                            var tempStages = Class1.dbo.Stages.FirstOrDefault(s => s.Id_stages == tempOrders.Id_stages);
                            var remainingStages = Class1.dbo.Stages.Where(s => s.Id_stages != tempStages.Id_stages).Select(s => s.Name);
                            string remainingStagesList = string.Join(", ", remainingStages);
                            var employeeNotification = new db.Notification()
                            {
                                Message = $"Вы перешли на следующий этап обработки заказа {tempOrders.Id_orders}" +
                             $"Этап обработки: {tempStages.Name}. " +
                             $"Оставшиеся этапы: {remainingStagesList}.",
                                Date = DateTime.Now,
                                Id_orders = tempOrders.Id_orders,
                                IsRead = false
                            };
                        }
                        else
                        {
                            MessageBox.Show("Достигнут последний этап.");
                        }
                    }
                    else if (EditComboBox.Text == "Status")
                    {
                        List<Status> statuses = Class1.dbo.Status.ToList();
                        int currentStatusIndex = statuses.FindIndex(s => s.Id_status == selectedOrder.Id_status);
                        if (selectedOrder.Id_stages >= 6 || selectedOrder.Id_status < 2)
                        {
                            if (currentStatusIndex >= 0 && currentStatusIndex < statuses.Count - 1)
                            {
                                selectedOrder.Id_status = statuses[currentStatusIndex + 1].Id_status;
                                var tempStatus = Class1.dbo.Status.FirstOrDefault(s=>s.Id_status==selectedOrder.Id_status);
                                var userNotification = new db.Notification()
                                {
                                    Message = $"Статус вашего заказа номер {tempOrders.Id_orders} изменился на {tempStatus.Name}",
                                    Date = DateTime.Now,
                                    Id_orders = tempOrders.Id_orders,
                                    IsRead = false,
                                };
                            }
                            else
                            {
                                MessageBox.Show("Достигнут последний статус.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Статус не может быть увеличен, пока этап меньше 6.");
                        }
                    }

                    Class1.dbo.SaveChanges();
                    OrdersDataGrid.ItemsSource = Class1.dbo.Orders.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для изменения.");
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            var orders = Class1.dbo.Orders.AsQueryable();

            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.Date <= endDate.Value);
            }

            OrdersDataGrid.ItemsSource = orders.ToList();
        }

        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProductName = ProductComboBox.SelectedItem as string;

            var orderDetails = Class1.dbo.Order_details.AsQueryable();

            if (!string.IsNullOrEmpty(selectedProductName))
            {
                orderDetails = orderDetails.Where(od => od.Product.Name == selectedProductName);
            }

            OrdersDetailsDataGrid.ItemsSource = orderDetails.ToList();
        }

        private void filterResetDetais_Click(object sender, RoutedEventArgs e)
        {
            ProductComboBox.SelectedIndex = -1;
            OrdersDetailsDataGrid.ItemsSource = null;
            OrdersDetailsDataGrid.Items.Clear();
            OrdersDetailsDataGrid.ItemsSource = Class1.dbo.Order_details.ToList();
        }
    }
}
