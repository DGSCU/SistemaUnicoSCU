//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScheduleAgent.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bando
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bando()
        {
            this.Progetti = new HashSet<Progetto>();
        }
    
        public int Gruppo { get; set; }
        public string Descrizione { get; set; }
        public string Scadenza { get; set; }
        public System.DateTime DataScadenza { get; set; }
        public Nullable<System.DateTime> DataScadenzaGraduatorie { get; set; }
        public Nullable<int> GiorniPostScadenza { get; set; }
        public Nullable<bool> programmi { get; set; }
        public Nullable<System.DateTime> DataFineAnnullamento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Progetto> Progetti { get; set; }
    }
}
