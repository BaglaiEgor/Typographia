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
    
    public partial class Transactions
    {
        public int Id_transactions { get; set; }
        public Nullable<int> Id_client { get; set; }
        public Nullable<decimal> TransactionAmount { get; set; }
        public string TransactionType { get; set; }
    
        public virtual Clients Clients { get; set; }
    }
}