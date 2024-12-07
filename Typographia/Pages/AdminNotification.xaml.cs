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
    /// Логика взаимодействия для AdminNotification.xaml
    /// </summary>
    public partial class AdminNotification : Page
    {
        public AdminNotification()
        {
            InitializeComponent();
            LoadNotification();
        }
        private void NotificationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = NotificationsDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                var selectedOrder = selectedItem as Notification;
                textNadpis.Visibility = Visibility.Visible;
                var tempNotif = Class1.dbo.Notification.FirstOrDefault(n => n.Id_notification == selectedOrder.Id_notification);
                textMessage.Text = tempNotif.Message;
                LoadNotification();
            }
            else
            {
                textNadpis.Visibility = Visibility.Hidden;
                textMessage.Text = null;
            }
        }
        private void LoadNotification()
        {
            string query = @"
    SELECT 
    n.Id_notification,
    n.Id_orders,
    n.Id_material,
    n.Date,
    n.Message,
    n.IsRead
FROM 
    Notification n;";

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var notificationlist = new List<Notification>();

                    while (reader.Read())
                    {
                        var notification = new Notification
                        {
                            Id_notification = reader.GetInt32(0),
                            Id_orders = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                            Id_material = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Date = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                            Message = reader.IsDBNull(4) ? null : reader.GetString(4),
                            IsRead = reader.IsDBNull(5) ? (bool?)null : reader.GetBoolean(5)
                        };

                        notificationlist.Add(notification);
                    }

                    NotificationsDataGrid.ItemsSource = null;
                    NotificationsDataGrid.Items.Clear();
                    NotificationsDataGrid.ItemsSource = notificationlist;
                }
            }
        }
    }
}
