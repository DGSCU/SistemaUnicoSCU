namespace DomandeOnline.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Motivazione
    {
        public Motivazione()
        {
            this.DomandaPartecipazione = new HashSet<DomandaPartecipazione>();
        }
    
        public int Id { get; set; }
        public string Descrizione { get; set; }
    
        public virtual ICollection<DomandaPartecipazione> DomandaPartecipazione { get; set; }
    }
}
