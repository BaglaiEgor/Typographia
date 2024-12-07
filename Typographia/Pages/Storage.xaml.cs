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

namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Page
    {
        public Storage()
        {
            InitializeComponent();
            MaterialsDataGrid.ItemsSource = null;
            MaterialsDataGrid.Items.Clear();
            MaterialsDataGrid.ItemsSource = Class1.dbo.Materials.ToList();
            LoadEquipment();
            LoadProduct();
            LoadRequirement();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempMaterial = new db.Materials()
                {
                    Name = txtNameM.Text,
                    Min_kolichestvo = Convert.ToInt32(txtMin.Text),
                    Count = Convert.ToInt32(txtCount.Text),
                    Srok_godnosti = Convert.ToDateTime(txtSrok.Text),
                    Cost = Convert.ToInt32(txtCostM.Text)
                };
                Class1.dbo.Materials.Add(tempMaterial);
                Class1.dbo.SaveChanges();
                MessageBox.Show("Материал успешно добавлен!");
                MaterialsDataGrid.ItemsSource = null;
                MaterialsDataGrid.Items.Clear();
                MaterialsDataGrid.ItemsSource = Class1.dbo.Materials.ToList();
            }
            catch
            {
                MessageBox.Show("Некорректно заполнены поля");
            }
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e) //Регламент
        {
            try
            {
                var tempRequirement = new db.ProductMaterialRequirement()
                {
                    Id_product = Convert.ToInt32(txtIdRp.Text),
                    Id_materials = Convert.ToInt32(txtIdRm.Text),
                    Id_equipment = Convert.ToInt32(txtIdEq.Text),
                    Quantity = Convert.ToInt32(txtQuantityR.Text)
                };
                Class1.dbo.ProductMaterialRequirement.Add(tempRequirement);
                Class1.dbo.SaveChanges();
                MessageBox.Show("Регламент успешно добавлен!");
                var material = Class1.dbo.Materials.FirstOrDefault(m => m.Id_materials == tempRequirement.Id_materials);
                if (material != null)
                {
                    var product = Class1.dbo.Product.FirstOrDefault(p => p.Id_product == tempRequirement.Id_product);
                    if (product != null)
                    {
                        product.Cost += material.Cost * tempRequirement.Quantity;

                        Class1.dbo.SaveChanges();
                        MessageBox.Show($"Цена продукта обновлена: {product.Cost}");
                    }
                    else
                    {
                        MessageBox.Show("Продукт не найден.");
                    }
                }
                else
                {
                    MessageBox.Show("Материал не найден.");
                }
                LoadProduct();
                LoadRequirement();
            }
            catch
            {
                MessageBox.Show("Некорректно заполнены поля");
            }
        }
        private void LoadRequirement()
        {
            string query = @"
    SELECT 
        r.Id_requirement,
        p.Name AS Name_product,
        m.Name AS Name_material,
        e.Name AS Name_equipment,
        r.Quantity
    FROM 
        ProductMaterialRequirement r
    JOIN
        Product p on p.Id_product = r.Id_product
    JOIN
        Materials m on m.Id_materials = r.Id_materials
    JOIN
        Equipment e on e.Id_equipment = r.Id_equipment";

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var RequirementFull = new List<object>();

                    while (reader.Read())
                    {
                        var requirement = new
                        {
                            Id_requirement = reader.GetInt32(0),
                            Name_product = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Name_material = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Name_equipment = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Quantity = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                        };

                        RequirementFull.Add(requirement);
                    }

                    RequirementDataGrid.ItemsSource = null;
                    RequirementDataGrid.Items.Clear();
                    RequirementDataGrid.ItemsSource = RequirementFull;
                }
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e) //Продукт
        {
            try
            {
                var tempProduct = new db.Product()
                {
                    Name = txtNameP.Text,
                    Cost = Convert.ToInt32(txtCostP.Text)
                };
                Class1.dbo.Product.Add(tempProduct);
                Class1.dbo.SaveChanges();
                MessageBox.Show("Продукт успешно добавлен!");
                LoadProduct();
            }
            catch
            {
                MessageBox.Show("Некорректно заполнены поля");
            }
        }
        private void LoadProduct()
        {
            string query = @"
    SELECT 
        p.Id_product,
        p.Name,
        p.Cost
    FROM 
        Product p";

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
                        };

                        ProductEquipment.Add(prodect);
                    }

                    ProductDataGrid.ItemsSource = null;
                    ProductDataGrid.Items.Clear();
                    ProductDataGrid.ItemsSource = ProductEquipment;
                }
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e) //Оборудование
        {
            try
            {
                var tempEquipment = new db.Equipment()
                {
                    Name = txtNameEq.Text,
                    Id_Type = Convert.ToInt32(txtTypeId.Text),
                    LoadPercentage = 0
                };
                Class1.dbo.Equipment.Add(tempEquipment);
                Class1.dbo.SaveChanges();
                MessageBox.Show("Оборудование успешно добавлено!");
                LoadEquipment();
            }
            catch
            {
                MessageBox.Show("Некорректно заполнены поля");
            }
        }
        private void LoadEquipment()
        {
            string query = @"
    SELECT 
        e.Id_equipment,
        e.Name,
        te.Name AS Name_type,
        e.LoadPercentage
    FROM 
        Equipment e
    JOIN 
        Type_equipment te ON te.Id_Type = e.Id_Type";

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var EquipmentwitType = new List<object>();

                    while (reader.Read())
                    {
                        var equipment = new
                        {
                            Id_equipment = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Name_type = reader.IsDBNull(2) ? null : reader.GetString(2),
                            LoadPercentage = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3),
                        };

                        EquipmentwitType.Add(equipment);
                    }

                    EquipmentDataGrid.ItemsSource = null;
                    EquipmentDataGrid.Items.Clear();
                    EquipmentDataGrid.ItemsSource = EquipmentwitType;
                }
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtIdM.Text, out int materialId) && int.TryParse(txtCountChange.Text, out int countChange))
            {
                var tempMaterials = Class1.dbo.Materials.FirstOrDefault(m => m.Id_materials == materialId);
                if (tempMaterials != null)
                {
                    if (countChange > 0)
                    {
                        tempMaterials.Count =  tempMaterials.Count+countChange;
                        MaterialsDataGrid.ItemsSource = null;
                        MaterialsDataGrid.Items.Clear();
                        MaterialsDataGrid.ItemsSource = Class1.dbo.Materials.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите число больше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Материал не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
