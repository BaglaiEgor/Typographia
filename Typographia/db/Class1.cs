using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typographia.db
{
    internal class Class1
    {
        public static string ConnectionString = "Server=DESKTOP-GNOPCJI\\SQLEXPRESS;Database=Tipografiya;Integrated Security=True;";
        public static TipografiyaEntities11 dbo = new TipografiyaEntities11();
        public static Transactions transactions;
        public static Clients clients;
        public static Orders orders;
        public static Stages stages;
        public static Status status;
        public static Order_details order_details;
        public static Notification notification;
        public static Employee employee;
        public static Product product;
        public static Equipment equipment;
        public static Type_equipment type_Equipment;
        public static ProductMaterialRequirement materialRequirement;
        public static Materials materials;
    }
}
