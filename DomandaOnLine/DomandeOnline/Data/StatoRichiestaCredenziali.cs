//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DomandeOnline.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class StatoRichiestaCredenziali
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StatoRichiestaCredenziali()
        {
            this.RichiestaCredenziali = new HashSet<RichiestaCredenziali>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RichiestaCredenziali> RichiestaCredenziali { get; set; }
    }
}
