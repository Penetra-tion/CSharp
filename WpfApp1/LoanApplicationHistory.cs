//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoanApplicationHistory
    {
        public int ID { get; set; }
        public int LoanApplicationID { get; set; }
        public string Status { get; set; }
        public string ChangedBy { get; set; }
        public System.DateTime ChangedDate { get; set; }
    
        public virtual LoanApplications LoanApplications { get; set; }
    }
}