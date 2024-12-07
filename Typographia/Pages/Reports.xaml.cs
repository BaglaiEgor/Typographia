using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Typographia.db;
using static Typographia.Pages.Reports;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using System.Data.SqlClient;



namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();
        }
        public class ResourceConsumption
        {
            public string MaterialName { get; set; }
            public int TotalUsed { get; set; }
            public decimal TotalCost { get; set; }
        }

        public List<ResourceConsumption> CalculateResourceConsumption()
        {
            var materials = Class1.dbo.Materials.ToList();

            var orderDetails = Class1.dbo.Order_details.ToList();

            var materialRequirements = Class1.dbo.ProductMaterialRequirement.ToList();

            var resourceConsumption = new List<ResourceConsumption>();

            foreach (var orderDetail in orderDetails)
            {
                var productRequirements = materialRequirements
                    .Where(req => req.Id_product == orderDetail.Id_product)
                    .ToList();

                foreach (var requirement in productRequirements)
                {
                    var material = materials.FirstOrDefault(m => m.Id_materials == requirement.Id_materials);
                    if (material != null)
                    {
                        int totalUsed = (requirement.Quantity ?? 0) * (orderDetail.Quantity ?? 0);
                        decimal totalCost = totalUsed * (material.Cost ?? 0);

                        var existingConsumption = resourceConsumption.FirstOrDefault(rc => rc.MaterialName == material.Name);
                        if (existingConsumption != null)
                        {
                            existingConsumption.TotalUsed += totalUsed;
                            existingConsumption.TotalCost += totalCost;
                        }
                        else
                        {
                            resourceConsumption.Add(new ResourceConsumption
                            {
                                MaterialName = material.Name,
                                TotalUsed = totalUsed,
                                TotalCost = totalCost
                            });
                        }
                    }
                }
            }

            return resourceConsumption;
        }
        public void ShowResourceConsumptionReport()
        {
            var resourceConsumption = CalculateResourceConsumption();

            if (resourceConsumption.Count == 0)
            {
                MessageBox.Show("Нет данных для отображения.");
                return;
            }

            StringBuilder reportBuilder = new StringBuilder();
            foreach (var consumption in resourceConsumption)
            {
                reportBuilder.AppendLine($"Материал: {consumption.MaterialName}, Всего использовано: {consumption.TotalUsed}, Общая стоимость: {consumption.TotalCost:C}");
            }

            MessageBox.Show(reportBuilder.ToString(), "Отчет о потреблении ресурсов");
        }
        private void buttRes_Click(object sender, RoutedEventArgs e)
        {
            ShowResourceConsumptionReport();

        }
            public class Report
            {
                public int TotalOrders { get; set; }
                public decimal TotalCost { get; set; }
                public List<EquipmentUsage> EquipmentUsage { get; set; }
                public List<ResourceConsumption> ResourceConsumption { get; set; }
            }

            public class EquipmentUsage
            {
                public string EquipmentName { get; set; }
                public decimal TotalLoad { get; set; }
            }

            public Report GenerateReport()
            {

                    var report = new Report();

                    report.TotalOrders = Class1.dbo.Orders.Count();

                    report.TotalCost = Class1.dbo.Orders.Sum(o => (o.Total_cost ?? 0));

                    report.EquipmentUsage = Class1.dbo.Equipment
                        .Select(eq => new EquipmentUsage
                        {
                            EquipmentName = eq.Name,
                            TotalLoad = (eq.LoadPercentage ?? 0)
                        }).ToList();

                    report.ResourceConsumption = Class1.dbo.ProductMaterialRequirement
                        .GroupBy(r => r.Id_materials)
                        .Select(g => new ResourceConsumption
                        {
                            MaterialName = Class1.dbo.Materials.FirstOrDefault(m => m.Id_materials == g.Key).Name,
                            TotalUsed = g.Sum(r => (r.Quantity ?? 0))
                        }).ToList();

                    return report;
            }
        public void ShowReportInMessageBox(Report report)
        {
            if (report == null)
            {
                MessageBox.Show("Отчет не может быть null", "Ошибка", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
                return;
            }

            StringBuilder reportText = new StringBuilder();
            reportText.AppendLine("Отчет о заказах");
            reportText.AppendLine($"Общее количество заказов: {report.TotalOrders}");
            reportText.AppendLine($"Общая стоимость: {report.TotalCost}");
            reportText.AppendLine();

            reportText.AppendLine("Загруженность оборудования:");
            foreach (var equipment in report.EquipmentUsage)
            {
                reportText.AppendLine($"Оборудование: {equipment.EquipmentName}, Загруженность: {equipment.TotalLoad}%");
            }

            reportText.AppendLine();
            reportText.AppendLine("Потребление ресурсов:");
            foreach (var resource in report.ResourceConsumption)
            {
                reportText.AppendLine($"Материал: {resource.MaterialName}, Общее количество: {resource.TotalUsed}");
            }

            MessageBox.Show(reportText.ToString(), "Отчет", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Information);
        }
        private void buttWork_Click(object sender, RoutedEventArgs e)
        {
            Report report = GenerateReport();
            ShowReportInMessageBox(report);
        }
        public class OrderData
        {
            public DateTime Date { get; set; }
            public float OrderVolume { get; set; }
            public float ResourceConsumption { get; set; }
        }
        private List<OrderData> orderDataList;
        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = Class1.ConnectionString;
            orderDataList = GetOrderData(connectionString);
            MessageBox.Show("Данные загружены.");
        }

        private List<OrderData> GetOrderData(string connectionString)
        {
            var orderDataList = new List<OrderData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT o.Date, 
                           SUM(od.Quantity) AS OrderVolume,
                           SUM(pm.Quantity) AS ResourceConsumption
                    FROM Orders o
                    JOIN Order_details od ON o.Id_orders = od.Id_orders
                    JOIN ProductMaterialRequirement pm ON od.Id_product = pm.Id_product
                    GROUP BY o.Date
                    ORDER BY o.Date;";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderDataList.Add(new OrderData
                        {
                            Date = reader.GetDateTime(0),
                            OrderVolume = reader.IsDBNull(1) ? 0 : (float)reader.GetFloat(1), // Преобразование из decimal в float
                            ResourceConsumption = reader.IsDBNull(2) ? 0 : (float)reader.GetFloat(2) // Преобразование из decimal в float
                        });
                    }
                }
            }

            return orderDataList;
        }

        private void btnPredict_Click(object sender, RoutedEventArgs e)
        {
            if (orderDataList == null || orderDataList.Count == 0)
            {
                MessageBox.Show("Сначала загрузите данные.");
                return;
            }

            // Простой прогноз на основе линейной регрессии
            var (slope, intercept) = CalculateLinearRegression(orderDataList);
            var forecastDate = DateTime.Now.AddDays(1); // Прогноз на следующий день
            var predictedVolume = slope * (float)(forecastDate - orderDataList[0].Date).TotalDays + intercept;

            txtOutput.Clear();
            txtOutput.AppendText($"Прогноз объемов заказов на {forecastDate.ToShortDateString()}: {predictedVolume}\n");
        }

        private (float slope, float intercept) CalculateLinearRegression(List<OrderData> data)
        {
            int n = data.Count;
            float sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                float x = (float)(data[i].Date - data[0].Date).TotalDays; // Преобразование даты в число
                float y = data[i].OrderVolume;

                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            float slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            float intercept = (sumY - slope * sumX) / n;

            return (slope, intercept);
        }
    }
}
