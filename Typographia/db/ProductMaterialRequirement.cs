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
    
    public partial class ProductMaterialRequirement
    {
        public int Id_requirement { get; set; }
        public Nullable<int> Id_product { get; set; }
        public Nullable<int> Id_materials { get; set; }
        public Nullable<int> Id_equipment { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual Equipment Equipment { get; set; }
        public virtual Materials Materials { get; set; }
        public virtual Product Product { get; set; }
    }
}