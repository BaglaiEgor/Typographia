using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Typographia.db;

namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Order : Page
    {
        string login;
        public Order(string LOGIN)
        {
            InitializeComponent();
            this.login = LOGIN;
            LoadProduct();
            LoadOrders();
            var tempUser = Class1.dbo.Clients.FirstOrDefault(u=>u.Login==login);
        }

        private void LoadProduct()
        {
            string query = @"
    SELECT 
        p.Id_product,
        p.Name,
        p.Cost,
COALESCE(MIN(m.Count / r.Quantity), 0) AS MaxProductionQuantity
        FROM 
            Product p
        JOIN 
            ProductMaterialRequirement r ON p.Id_product = r.Id_product
        JOIN 
            Materials m ON r.Id_materials = m.Id_materials
        GROUP BY 
            p.Id_product, p.Name, p.Cost
        HAVING 
            MIN(m.Count / r.Quantity) > 0;";
            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var ProductEquipment = new List<object>();

                    while (reader.Read())
                    {
                        var prodect = new
                        {
                            Id_product = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Cost = reader.IsDBNull(2) ? (decimal?)null : reader.GetDecimal(2),
                            MaxProductionQuantity = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)

                        };

                        ProductEquipment.Add(prodect);
                    }

                    ProductDataGrid.ItemsSource = null;
                    ProductDataGrid.Items.Clear();
                    ProductDataGrid.ItemsSource = ProductEquipment;
                }
            }
        }
        public class OrderViewModel
        {
            public int Id_orders { get; set; }
            public DateTime? Date { get; set; }
            public string Name_status { get; set; }
            public decimal? Total_cost { get; set; }
        }
        private void LoadOrders()
        {
            var tempUser = Class1.dbo.Clients.FirstOrDefault(u => u.Login == login);
            string query = @"
    SELECT 
        o.Id_orders,
        o.Date,
        s.Name AS Name_status,
        o.Total_cost
    FROM 
        Orders o
    JOIN 
        Status s on s.Id_status = o.Id_status
    WHERE
        o.Id_clients = @Id_Clients;";

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@Id_Clients", tempUser.Id_client);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var ordersList = new List<OrderViewModel>();

                    while (reader.Read())
                    {
                        var order = new OrderViewModel
                        {
                            Id_orders = reader.GetInt32(0),
                            Date = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1),
                            Name_status = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Total_cost = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3)
                        };

                        ordersList.Add(order);
                    }

                    OrdersDataGrid.ItemsSource = null;
                    OrdersDataGrid.Items.Clear();
                    OrdersDataGrid.ItemsSource = ordersList;
                }
            }
        }
        int IdOrder;

        private void LoadOrdersDetails(int idOrders)
        {
            string query = @"
SELECT 
    od.Id_orders,
    p.Name AS Name_product,
    od.Quantity,
    od.Size,
    od.Color_scheme,
    od.Cost
FROM 
    Order_details od
JOIN 
    Product p ON p.Id_product = od.Id_product
WHERE 
    od.Id_orders = @CurrentOrderId"; ;

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CurrentOrderId", idOrders);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var ordersDetailsList = new List<object>();

                    while (reader.Read())
                    {
                        var ordersDetails = new
                        {
                            Id_orders = reader.GetInt32(0),
                            Name_product = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Quantity = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Size = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            Color_scheme = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Cost = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5)
                        };

                        ordersDetailsList.Add(ordersDetails);
                    }
                    OrdersDetailsDataGrid.ItemsSource = null;
                    OrdersDetailsDataGrid.Items.Clear();
                    OrdersDetailsDataGrid.ItemsSource = ordersDetailsList;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempUser = Class1.dbo.Clients.FirstOrDefault(u => u.Login == login);
                var employees = Class1.dbo.Employee.ToList();
                Random random = new Random();
                int randomIndex = random.Next(employees.Count);
                var tempOrders = new db.Orders()
                {
                    Date = DateTime.Now,
                    Id_clients = tempUser.Id_client,
                    Id_employee = employees[randomIndex].Id_employee,
                    Id_stages = 1,
                    Id_status = 2,
                    Total_cost = 0
                };
                Class1.dbo.Orders.Add(tempOrders);
                butDetails.IsEnabled = true;
                MessageBox.Show("Заказ успешно добавлен!");
                Class1.dbo.SaveChanges();
                IdOrder = tempOrders.Id_orders;
                LoadOrders();
                var tempStatus = Class1.dbo.Status.FirstOrDefault(s => s.Id_status == tempOrders.Id_status);
                var userNotification = new db.Notification()
                {
                    Message = $"Ваш заказ номер {tempOrders.Id_orders} успешно оформлен. Статус: {tempStatus.Name}",
                    Date = DateTime.Now,
                    Id_orders = tempOrders.Id_orders,
                    IsRead = false,
                };
                Class1.dbo.Notification.Add(userNotification);
                var tempStages = Class1.dbo.Stages.FirstOrDefault(s => s.Id_stages == tempOrders.Id_stages);
                var remainingStages = Class1.dbo.Stages.Where(s => s.Id_stages != tempStages.Id_stages).Select(s => s.Name);
                string remainingStagesList = string.Join(", ", remainingStages);
                var employeeNotification = new db.Notification()
                {
                   Message = $"Вам назначен новый заказ от {tempUser.FirstName} {tempUser.LastName}. " +
                 $"Этап обработки: {tempStages.Name}. " +
                 $"Оставшиеся этапы: {remainingStagesList}.",
                    Date = DateTime.Now,
                    Id_orders = tempOrders.Id_orders,
                    IsRead = false
                };
                Class1.dbo.Notification.Add(employeeNotification);
                Class1.dbo.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Некорректно заполнены поля");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string NameProduct = txtProduct.Text;
                var tempProduct = Class1.dbo.Product.FirstOrDefault(u => u.Name == NameProduct);
                var tempOrders = Class1.dbo.Orders.FirstOrDefault(o => o.Id_orders == IdOrder);

                if (!string.IsNullOrWhiteSpace(txtProduct.Text) &&
                    !string.IsNullOrWhiteSpace(txtCount.Text) &&
                    !string.IsNullOrWhiteSpace(txtGama.Text) &&
                    !string.IsNullOrWhiteSpace(txtSize.Text))
                {
                    if (tempProduct != null)
                    {
                        if (tempOrders != null)
                        {
                            int quantityToProduce = Convert.ToInt32(txtCount.Text);
                            var materialRequirements = Class1.dbo.ProductMaterialRequirement
                                .Where(r => r.Id_product == tempProduct.Id_product)
                                .Select(r => new
                                {
                                    r.Id_materials,
                                    r.Id_equipment,
                                    RequiredQuantity = r.Quantity * quantityToProduce
                                })
                                .ToList();

                            bool canProduce = true;
                            int totalMaterialUsed = 0;
                            int totalEquipmentLoadIncrease = 0;

                            foreach (var requirement in materialRequirements)
                            {
                                var materialOnStock = Class1.dbo.Materials
                                    .Where(m => m.Id_materials == requirement.Id_materials)
                                    .Select(m => new { m.Count, m.Min_kolichestvo })
                                    .FirstOrDefault();

                                if (materialOnStock == null || materialOnStock.Count < requirement.RequiredQuantity)
                                {
                                    canProduce = false;
                                    break;
                                }

                                totalMaterialUsed += Convert.ToInt32(requirement.RequiredQuantity);
                                totalEquipmentLoadIncrease += Convert.ToInt32(requirement.RequiredQuantity) / 4;
                            }

                            if (canProduce)
                            {
                                var tempDetails = new db.Order_details()
                                {
                                    Id_orders = IdOrder,
                                    Id_product = tempProduct.Id_product,
                                    Quantity = quantityToProduce,
                                    Size = Convert.ToInt32(txtSize.Text),
                                    Color_scheme = txtGama.Text,
                                    Cost = tempProduct.Cost * quantityToProduce
                                };
                                tempOrders.Total_cost += tempDetails.Cost;
                                Class1.dbo.Order_details.Add(tempDetails);

                                foreach (var requirement in materialRequirements)
                                {
                                    var material = Class1.dbo.Materials.First(m => m.Id_materials == requirement.Id_materials);
                                    material.Count -= requirement.RequiredQuantity;

                                    if (material.Count <= material.Min_kolichestvo)
                                    {
                                        var notification = new db.Notification()
                                        {
                                            Id_material = material.Id_materials,
                                            Message = $"ВНИМАНИЕ: Минимальный запас материала '{material.Name}' достигнут.",
                                            Date = DateTime.Now
                                        };
                                        Class1.dbo.Notification.Add(notification);
                                    }
                                }
                                var equipmentLoadUpdates = materialRequirements
                                    .GroupBy(r => r.Id_equipment)
                                    .Select(g => new
                                    {
                                        Id_equipment = g.Key,
                                        LoadIncrease = g.Sum(r => r.RequiredQuantity) / 4
                                    });

                                bool loadExceedsLimit = false;

                                foreach (var equipmentLoadUpdate in equipmentLoadUpdates)
                                {
                                    var equipment = Class1.dbo.Equipment.FirstOrDefault(a => a.Id_equipment == equipmentLoadUpdate.Id_equipment);
                                    if (equipment != null)
                                    {
                                        int newLoad = Convert.ToInt32(equipment.LoadPercentage) + Convert.ToInt32(equipmentLoadUpdate.LoadIncrease);
                                        if (newLoad > 100)
                                        {
                                            loadExceedsLimit = true;
                                            break;
                                        }
                                        equipment.LoadPercentage = newLoad;
                                    }
                                }

                                if (loadExceedsLimit)
                                {
                                    MessageBox.Show("Нагрузки оборудования превышают допустимый предел (100).");
                                }
                                else
                                {
                                    Class1.dbo.SaveChanges();
                                    MessageBox.Show("Продукт успешно добавлен в заказ!");
                                    LoadOrders();
                                    LoadProduct();
                                    LoadOrdersDetails(IdOrder);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Мы не можем сделать столько данного продукта из-за нехватки материалов на складе.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Заказ не найден.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данного продукта нет в ассортименте");
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = OrdersDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                var selectedOrder = selectedItem as OrderViewModel;

                if (selectedOrder != null)
                {
                    IdOrder = selectedOrder.Id_orders;
                    if (!string.IsNullOrEmpty(Convert.ToString(IdOrder)))
                    {
                        LoadOrdersDetails(IdOrder);
                    }
                    else
                    {
                        MessageBox.Show("ID заказа не установлен.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите действительный заказ.");
            }
        }
    }
}
