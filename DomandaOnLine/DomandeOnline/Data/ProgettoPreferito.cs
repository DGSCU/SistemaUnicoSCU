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
    
    public partial class ProgettoPreferito
    {
        public string CodiceFiscale { get; set; }
        public string CodiceProgetto { get; set; }
        public int CodiceSede { get; set; }
    
        public virtual Progetto Progetti { get; set; }
    }
}
