using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
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
using static Typographia.Pages.Order;

namespace Typographia.Pages
{
    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    public partial class Notifications : Page
    {
        string login;
        public Notifications(string LOGIN)
        {
            InitializeComponent();
            this.login = LOGIN;
            var tempUser = Class1.dbo.Clients.FirstOrDefault(c => c.Login == login);
            LoadNotification();
        }
        public class NotificationView
        {
            public int Id_notification { get; set; }
            public DateTime? Date { get; set; }
            public string Message { get; set; }
            public bool? IsRead { get; set; }
        }
        private void LoadNotification()
        {
            var tempUser = Class1.dbo.Clients.FirstOrDefault(c => c.Login == login);
            string query = @"
    SELECT 
        n.Id_notification,
        n.Date,
        n.Message,
        n.IsRead
    FROM 
        Notification n
    JOIN 
        Orders o on n.Id_orders = o.Id_orders
    WHERE
        o.Id_clients = @Id_client AND n.Message not like 'Вы перешли на следующий этап%' AND n.Message not like 'Вам назначен%';";

            using (SqlConnection connection = new SqlConnection(Class1.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@Id_client", tempUser.Id_client);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var notificationlist = new List<NotificationView>();

                    while (reader.Read())
                    {
                        var notification = new NotificationView
                        {
                            Id_notification = reader.GetInt32(0),
                            Date = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1),
                            Message = reader.IsDBNull(2) ? null : reader.GetString(2),
                            IsRead = reader.IsDBNull(3) ? (bool?)null : reader.GetBoolean(3)
                        };

                        notificationlist.Add(notification);
                    }

                    NotificationsDataGrid.ItemsSource = null;
                    NotificationsDataGrid.Items.Clear();
                    NotificationsDataGrid.ItemsSource = notificationlist;
                }
            }
        }

        private void NotificationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = NotificationsDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                var selectedOrder = selectedItem as NotificationView;
                textNadpis.Visibility = Visibility.Visible;
                var tempNotif = Class1.dbo.Notification.FirstOrDefault(n => n.Id_notification == selectedOrder.Id_notification);
                textMessage.Text = tempNotif.Message;
                tempNotif.IsRead = true;
                Class1.dbo.SaveChanges();
                LoadNotification();
            }
            else
            {
                textNadpis.Visibility = Visibility.Hidden;
                textMessage.Text = null;
            }
        }
    }
}
