//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Typographia.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order_details
    {
        public int Id_details { get; set; }
        public Nullable<int> Id_orders { get; set; }
        public Nullable<int> Id_product { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Size { get; set; }
        public string Color_scheme { get; set; }
        public Nullable<decimal> Cost { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual Product Product { get; set; }
    }
}
