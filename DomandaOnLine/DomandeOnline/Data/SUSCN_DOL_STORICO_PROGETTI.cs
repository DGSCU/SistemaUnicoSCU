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
    
    public partial class SUSCN_DOL_STORICO_PROGETTI
    {
        public Nullable<System.DateTime> DataStorico { get; set; }
        public string CodiceEnte { get; set; }
        public string NomeEnte { get; set; }
        public string Sito { get; set; }
        public string CodiceProgetto { get; set; }
        public string TitoloProgetto { get; set; }
        public string TipoProgetto { get; set; }
        public int CodiceSede { get; set; }
        public string IndirizzoSede { get; set; }
        public string Regione { get; set; }
        public string Provincia { get; set; }
        public string Comune { get; set; }
        public string Settore { get; set; }
        public string Area { get; set; }
        public Nullable<short> NumeroPostiDisponibili { get; set; }
        public int Gruppo { get; set; }
        public string Misure { get; set; }
        public byte DurataProgettoMesi { get; set; }
        public short NumeroGiovaniMinoriOpportunità { get; set; }
        public string EsteroUE { get; set; }
        public string Tutoraggio { get; set; }
        public Nullable<System.DateTime> DataAnnullamento { get; set; }
        public Nullable<int> IDParticolaritàEntità { get; set; }
        public Nullable<int> IdProgramma { get; set; }
        public string LinkSintesi { get; set; }
        public string EnteAttuatore { get; set; }
    }
}