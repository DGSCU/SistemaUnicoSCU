using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SIGED_WSSoap
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_ANAGRAFICA", Namespace = "http://tempuri.org/")]
    public partial class MULTI_ANAGRAFICA : object
    {

        private ANAGRAFICA_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public ANAGRAFICA_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ANAGRAFICA_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class ANAGRAFICA_DOCUMENTO_TROVATO : object
    {

        private string CITTAField;

        private string CODICEANAGRAFICAField;

        private string INDIRIZZOField;

        private string NOMINATIVOField;

        private string PROVINCIAField;

        private string TIPOLOGIAField;

        private string CAPField;

        private string EMAILField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CITTA
        {
            get
            {
                return this.CITTAField;
            }
            set
            {
                this.CITTAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEANAGRAFICA
        {
            get
            {
                return this.CODICEANAGRAFICAField;
            }
            set
            {
                this.CODICEANAGRAFICAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string INDIRIZZO
        {
            get
            {
                return this.INDIRIZZOField;
            }
            set
            {
                this.INDIRIZZOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NOMINATIVO
        {
            get
            {
                return this.NOMINATIVOField;
            }
            set
            {
                this.NOMINATIVOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string PROVINCIA
        {
            get
            {
                return this.PROVINCIAField;
            }
            set
            {
                this.PROVINCIAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TIPOLOGIA
        {
            get
            {
                return this.TIPOLOGIAField;
            }
            set
            {
                this.TIPOLOGIAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string CAP
        {
            get
            {
                return this.CAPField;
            }
            set
            {
                this.CAPField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string EMAIL
        {
            get
            {
                return this.EMAILField;
            }
            set
            {
                this.EMAILField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "INDICE_ANAGRAFICA", Namespace = "http://tempuri.org/")]
    public partial class INDICE_ANAGRAFICA : object
    {

        private ANAGRAFICA_DOCUMENTO DATIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public ANAGRAFICA_DOCUMENTO DATI
        {
            get
            {
                return this.DATIField;
            }
            set
            {
                this.DATIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ANAGRAFICA_DOCUMENTO", Namespace = "http://tempuri.org/")]
    public partial class ANAGRAFICA_DOCUMENTO : object
    {

        private string AZIENDAField;

        private string CAPField;

        private string CITTAField;

        private string CODICEANAGRAFICAField;

        private string CODICEFISCALEField;

        private string CODICEUNIVOCOField;

        private string DATANASCITAField;

        private string EMAILField;

        private string FAXField;

        private string INDIRIZZOField;

        private string LUOGONASCITAField;

        private string NOMINATIVOField;

        private string PARTITAIVAField;

        private string PROVINCIAField;

        private string TIPOLOGIAField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string AZIENDA
        {
            get
            {
                return this.AZIENDAField;
            }
            set
            {
                this.AZIENDAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CAP
        {
            get
            {
                return this.CAPField;
            }
            set
            {
                this.CAPField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CITTA
        {
            get
            {
                return this.CITTAField;
            }
            set
            {
                this.CITTAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEANAGRAFICA
        {
            get
            {
                return this.CODICEANAGRAFICAField;
            }
            set
            {
                this.CODICEANAGRAFICAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEFISCALE
        {
            get
            {
                return this.CODICEFISCALEField;
            }
            set
            {
                this.CODICEFISCALEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEUNIVOCO
        {
            get
            {
                return this.CODICEUNIVOCOField;
            }
            set
            {
                this.CODICEUNIVOCOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DATANASCITA
        {
            get
            {
                return this.DATANASCITAField;
            }
            set
            {
                this.DATANASCITAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string EMAIL
        {
            get
            {
                return this.EMAILField;
            }
            set
            {
                this.EMAILField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string FAX
        {
            get
            {
                return this.FAXField;
            }
            set
            {
                this.FAXField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string INDIRIZZO
        {
            get
            {
                return this.INDIRIZZOField;
            }
            set
            {
                this.INDIRIZZOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string LUOGONASCITA
        {
            get
            {
                return this.LUOGONASCITAField;
            }
            set
            {
                this.LUOGONASCITAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NOMINATIVO
        {
            get
            {
                return this.NOMINATIVOField;
            }
            set
            {
                this.NOMINATIVOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string PARTITAIVA
        {
            get
            {
                return this.PARTITAIVAField;
            }
            set
            {
                this.PARTITAIVAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string PROVINCIA
        {
            get
            {
                return this.PROVINCIAField;
            }
            set
            {
                this.PROVINCIAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TIPOLOGIA
        {
            get
            {
                return this.TIPOLOGIAField;
            }
            set
            {
                this.TIPOLOGIAField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_FASCICOLO", Namespace = "http://tempuri.org/")]
    public partial class MULTI_FASCICOLO : object
    {

        private FASCICOLO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public FASCICOLO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "FASCICOLO_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class FASCICOLO_DOCUMENTO_TROVATO : object
    {

        private string CODICEFASCICOLOField;

        private string DESCRIZIONEField;

        private string NUMEROField;

        private string TITOLARIOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEFASCICOLO
        {
            get
            {
                return this.CODICEFASCICOLOField;
            }
            set
            {
                this.CODICEFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NUMERO
        {
            get
            {
                return this.NUMEROField;
            }
            set
            {
                this.NUMEROField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TITOLARIO
        {
            get
            {
                return this.TITOLARIOField;
            }
            set
            {
                this.TITOLARIOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "INDICE_FASCICOLO", Namespace = "http://tempuri.org/")]
    public partial class INDICE_FASCICOLO : object
    {

        private FASCICOLO_DOCUMENTO DATIField;

        private COLLEGAMENTO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public FASCICOLO_DOCUMENTO DATI
        {
            get
            {
                return this.DATIField;
            }
            set
            {
                this.DATIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public COLLEGAMENTO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "FASCICOLO_DOCUMENTO", Namespace = "http://tempuri.org/")]
    public partial class FASCICOLO_DOCUMENTO : object
    {

        private string CATEGORIAField;

        private string CODICEFASCICOLOField;

        private string CODICETITOLARIOField;

        private string DATAAPERTURAField;

        private string DESCRIZIONEField;

        private string NUMEROField;

        private string RIFERIMENTOField;

        private string STATOField;

        private string UNITAORGANIZZATIVARESPONSABILEField;

        private string WEBLINKField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CATEGORIA
        {
            get
            {
                return this.CATEGORIAField;
            }
            set
            {
                this.CATEGORIAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEFASCICOLO
        {
            get
            {
                return this.CODICEFASCICOLOField;
            }
            set
            {
                this.CODICEFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICETITOLARIO
        {
            get
            {
                return this.CODICETITOLARIOField;
            }
            set
            {
                this.CODICETITOLARIOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DATAAPERTURA
        {
            get
            {
                return this.DATAAPERTURAField;
            }
            set
            {
                this.DATAAPERTURAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NUMERO
        {
            get
            {
                return this.NUMEROField;
            }
            set
            {
                this.NUMEROField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string RIFERIMENTO
        {
            get
            {
                return this.RIFERIMENTOField;
            }
            set
            {
                this.RIFERIMENTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string STATO
        {
            get
            {
                return this.STATOField;
            }
            set
            {
                this.STATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string UNITAORGANIZZATIVARESPONSABILE
        {
            get
            {
                return this.UNITAORGANIZZATIVARESPONSABILEField;
            }
            set
            {
                this.UNITAORGANIZZATIVARESPONSABILEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string WEBLINK
        {
            get
            {
                return this.WEBLINKField;
            }
            set
            {
                this.WEBLINKField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "COLLEGAMENTO_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class COLLEGAMENTO_DOCUMENTO_TROVATO : object
    {

        private string CODICEDOCUMENTOCOLLEGATOField;

        private string DESCRIZIONECOLLEGAMENTOField;

        private string TIPOCOLLEGAMENTOField;

        private string FLAGAPERTURAField;

        private string DETTAGLICOLLEGAMENTOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEDOCUMENTOCOLLEGATO
        {
            get
            {
                return this.CODICEDOCUMENTOCOLLEGATOField;
            }
            set
            {
                this.CODICEDOCUMENTOCOLLEGATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONECOLLEGAMENTO
        {
            get
            {
                return this.DESCRIZIONECOLLEGAMENTOField;
            }
            set
            {
                this.DESCRIZIONECOLLEGAMENTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TIPOCOLLEGAMENTO
        {
            get
            {
                return this.TIPOCOLLEGAMENTOField;
            }
            set
            {
                this.TIPOCOLLEGAMENTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string FLAGAPERTURA
        {
            get
            {
                return this.FLAGAPERTURAField;
            }
            set
            {
                this.FLAGAPERTURAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string DETTAGLICOLLEGAMENTO
        {
            get
            {
                return this.DETTAGLICOLLEGAMENTOField;
            }
            set
            {
                this.DETTAGLICOLLEGAMENTOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "FASCICOLO_CREATO", Namespace = "http://tempuri.org/")]
    public partial class FASCICOLO_CREATO : object
    {

        private string CODICEFASCICOLOField;

        private string NUMEROFASCICOLOField;

        private string CODICEDOCUMENTOCOLLEGATOField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEFASCICOLO
        {
            get
            {
                return this.CODICEFASCICOLOField;
            }
            set
            {
                this.CODICEFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NUMEROFASCICOLO
        {
            get
            {
                return this.NUMEROFASCICOLOField;
            }
            set
            {
                this.NUMEROFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CODICEDOCUMENTOCOLLEGATO
        {
            get
            {
                return this.CODICEDOCUMENTOCOLLEGATOField;
            }
            set
            {
                this.CODICEDOCUMENTOCOLLEGATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "SOTTOFASCICOLO_CREATO", Namespace = "http://tempuri.org/")]
    public partial class SOTTOFASCICOLO_CREATO : object
    {

        private string TITOLARIOField;

        private string CODICESOTTOFASCICOLOField;

        private string NUMEROSOTTOFASCICOLOField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TITOLARIO
        {
            get
            {
                return this.TITOLARIOField;
            }
            set
            {
                this.TITOLARIOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICESOTTOFASCICOLO
        {
            get
            {
                return this.CODICESOTTOFASCICOLOField;
            }
            set
            {
                this.CODICESOTTOFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string NUMEROSOTTOFASCICOLO
        {
            get
            {
                return this.NUMEROSOTTOFASCICOLOField;
            }
            set
            {
                this.NUMEROSOTTOFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "FASCICOLOEX_CREATO", Namespace = "http://tempuri.org/")]
    public partial class FASCICOLOEX_CREATO : object
    {

        private string CODICEFASCICOLOField;

        private string NUMEROFASCICOLOField;

        private string CODICEDOCUMENTOCOLLEGATOField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEFASCICOLO
        {
            get
            {
                return this.CODICEFASCICOLOField;
            }
            set
            {
                this.CODICEFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NUMEROFASCICOLO
        {
            get
            {
                return this.NUMEROFASCICOLOField;
            }
            set
            {
                this.NUMEROFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CODICEDOCUMENTOCOLLEGATO
        {
            get
            {
                return this.CODICEDOCUMENTOCOLLEGATOField;
            }
            set
            {
                this.CODICEDOCUMENTOCOLLEGATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "SOTTOFASCICOLOEX_CREATO", Namespace = "http://tempuri.org/")]
    public partial class SOTTOFASCICOLOEX_CREATO : object
    {

        private string CODICEDOCUMENTOCOLLEGATOField;

        private string TITOLARIOField;

        private string CODICESOTTOFASCICOLOField;

        private string NUMEROSOTTOFASCICOLOField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEDOCUMENTOCOLLEGATO
        {
            get
            {
                return this.CODICEDOCUMENTOCOLLEGATOField;
            }
            set
            {
                this.CODICEDOCUMENTOCOLLEGATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TITOLARIO
        {
            get
            {
                return this.TITOLARIOField;
            }
            set
            {
                this.TITOLARIOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CODICESOTTOFASCICOLO
        {
            get
            {
                return this.CODICESOTTOFASCICOLOField;
            }
            set
            {
                this.CODICESOTTOFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string NUMEROSOTTOFASCICOLO
        {
            get
            {
                return this.NUMEROSOTTOFASCICOLOField;
            }
            set
            {
                this.NUMEROSOTTOFASCICOLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "DATO_MULTI", Namespace = "http://tempuri.org/")]
    public partial class DATO_MULTI : object
    {

        private MULTI_VALORE[] DATOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] DATO
        {
            get
            {
                return this.DATOField;
            }
            set
            {
                this.DATOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_VALORE", Namespace = "http://tempuri.org/")]
    public partial class MULTI_VALORE : object
    {

        private string VALOREField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string VALORE
        {
            get
            {
                return this.VALOREField;
            }
            set
            {
                this.VALOREField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_FASCICOLO_CREATO", Namespace = "http://tempuri.org/")]
    public partial class MULTI_FASCICOLO_CREATO : object
    {

        private FASCICOLO_CREATO[] ELENCO_FASCICOLIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public FASCICOLO_CREATO[] ELENCO_FASCICOLI
        {
            get
            {
                return this.ELENCO_FASCICOLIField;
            }
            set
            {
                this.ELENCO_FASCICOLIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "INDICE_ALLEGATO", Namespace = "http://tempuri.org/")]
    public partial class INDICE_ALLEGATO : object
    {

        private ALLEGATO_DOCUMENTO DATIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public ALLEGATO_DOCUMENTO DATI
        {
            get
            {
                return this.DATIField;
            }
            set
            {
                this.DATIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ALLEGATO_DOCUMENTO", Namespace = "http://tempuri.org/")]
    public partial class ALLEGATO_DOCUMENTO : object
    {

        private string BASE64Field;

        private string CODICEALLEGATOField;

        private string DESCRIZIONEField;

        private string NOMEFILEField;

        private string FULLPATHFILEField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string BASE64
        {
            get
            {
                return this.BASE64Field;
            }
            set
            {
                this.BASE64Field = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEALLEGATO
        {
            get
            {
                return this.CODICEALLEGATOField;
            }
            set
            {
                this.CODICEALLEGATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NOMEFILE
        {
            get
            {
                return this.NOMEFILEField;
            }
            set
            {
                this.NOMEFILEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string FULLPATHFILE
        {
            get
            {
                return this.FULLPATHFILEField;
            }
            set
            {
                this.FULLPATHFILEField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_PROTOCOLLO", Namespace = "http://tempuri.org/")]
    public partial class MULTI_PROTOCOLLO : object
    {

        private PROTOCOLLO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public PROTOCOLLO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PROTOCOLLO_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLO_DOCUMENTO_TROVATO : object
    {

        private string CODICEPROTOCOLLOField;

        private string CORRISPONDENTEField;

        private string INDIRIZZOField;

        private string DATAField;

        private string NUMEROField;

        private string OGGETTOField;

        private string STATOField;

        private string TIPOPROTOCOLLOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEPROTOCOLLO
        {
            get
            {
                return this.CODICEPROTOCOLLOField;
            }
            set
            {
                this.CODICEPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CORRISPONDENTE
        {
            get
            {
                return this.CORRISPONDENTEField;
            }
            set
            {
                this.CORRISPONDENTEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string INDIRIZZO
        {
            get
            {
                return this.INDIRIZZOField;
            }
            set
            {
                this.INDIRIZZOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string DATA
        {
            get
            {
                return this.DATAField;
            }
            set
            {
                this.DATAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string NUMERO
        {
            get
            {
                return this.NUMEROField;
            }
            set
            {
                this.NUMEROField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string OGGETTO
        {
            get
            {
                return this.OGGETTOField;
            }
            set
            {
                this.OGGETTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string STATO
        {
            get
            {
                return this.STATOField;
            }
            set
            {
                this.STATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string TIPOPROTOCOLLO
        {
            get
            {
                return this.TIPOPROTOCOLLOField;
            }
            set
            {
                this.TIPOPROTOCOLLOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "INDICE_PROTOCOLLO", Namespace = "http://tempuri.org/")]
    public partial class INDICE_PROTOCOLLO : object
    {

        private PROTOCOLLO_DOCUMENTO DATIField;

        private ALLEGATO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private FASCICOLO_DOCUMENTO_TROVATO[] ELENCO_FASCICOLIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public PROTOCOLLO_DOCUMENTO DATI
        {
            get
            {
                return this.DATIField;
            }
            set
            {
                this.DATIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public ALLEGATO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public FASCICOLO_DOCUMENTO_TROVATO[] ELENCO_FASCICOLI
        {
            get
            {
                return this.ELENCO_FASCICOLIField;
            }
            set
            {
                this.ELENCO_FASCICOLIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PROTOCOLLO_DOCUMENTO", Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLO_DOCUMENTO : object
    {

        private string ANNOField;

        private MULTI_VALORE[] CODICECORRISPONDENTIField;

        private string CODICEPROTOCOLLOField;

        private string CODICETITOLARIOField;

        private string DATAPROTOCOLLOField;

        private string DATAESTREMIField;

        private string ESTREMIField;

        private string NUMEROPROTOCOLLOField;

        private string OGGETTOField;

        private string STATOField;

        private string TIPODOCUMENTOField;

        private string TIPOPROTOCOLLOField;

        private string PROTOCOLLORIFERIMENTOField;

        private MULTI_VALORE[] UNITAORGANIZZATIVARESPONSABILIField;

        private string WEBLINKField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ANNO
        {
            get
            {
                return this.ANNOField;
            }
            set
            {
                this.ANNOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] CODICECORRISPONDENTI
        {
            get
            {
                return this.CODICECORRISPONDENTIField;
            }
            set
            {
                this.CODICECORRISPONDENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEPROTOCOLLO
        {
            get
            {
                return this.CODICEPROTOCOLLOField;
            }
            set
            {
                this.CODICEPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICETITOLARIO
        {
            get
            {
                return this.CODICETITOLARIOField;
            }
            set
            {
                this.CODICETITOLARIOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DATAPROTOCOLLO
        {
            get
            {
                return this.DATAPROTOCOLLOField;
            }
            set
            {
                this.DATAPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string DATAESTREMI
        {
            get
            {
                return this.DATAESTREMIField;
            }
            set
            {
                this.DATAESTREMIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string ESTREMI
        {
            get
            {
                return this.ESTREMIField;
            }
            set
            {
                this.ESTREMIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string NUMEROPROTOCOLLO
        {
            get
            {
                return this.NUMEROPROTOCOLLOField;
            }
            set
            {
                this.NUMEROPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string OGGETTO
        {
            get
            {
                return this.OGGETTOField;
            }
            set
            {
                this.OGGETTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string STATO
        {
            get
            {
                return this.STATOField;
            }
            set
            {
                this.STATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string TIPODOCUMENTO
        {
            get
            {
                return this.TIPODOCUMENTOField;
            }
            set
            {
                this.TIPODOCUMENTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string TIPOPROTOCOLLO
        {
            get
            {
                return this.TIPOPROTOCOLLOField;
            }
            set
            {
                this.TIPOPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string PROTOCOLLORIFERIMENTO
        {
            get
            {
                return this.PROTOCOLLORIFERIMENTOField;
            }
            set
            {
                this.PROTOCOLLORIFERIMENTOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public MULTI_VALORE[] UNITAORGANIZZATIVARESPONSABILI
        {
            get
            {
                return this.UNITAORGANIZZATIVARESPONSABILIField;
            }
            set
            {
                this.UNITAORGANIZZATIVARESPONSABILIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string WEBLINK
        {
            get
            {
                return this.WEBLINKField;
            }
            set
            {
                this.WEBLINKField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ALLEGATO_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class ALLEGATO_DOCUMENTO_TROVATO : object
    {

        private string CODICEALLEGATOField;

        private string DESCRIZIONEField;

        private string NOMEFILEField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEALLEGATO
        {
            get
            {
                return this.CODICEALLEGATOField;
            }
            set
            {
                this.CODICEALLEGATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NOMEFILE
        {
            get
            {
                return this.NOMEFILEField;
            }
            set
            {
                this.NOMEFILEField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PROTOCOLLO_CREATO", Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLO_CREATO : object
    {

        private string CODICEPROTOCOLLOField;

        private string NUMEROPROTOCOLLOField;

        private string DATAPROTOCOLLOField;

        private string ESITOField;

        private string CODICEALLEGATOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEPROTOCOLLO
        {
            get
            {
                return this.CODICEPROTOCOLLOField;
            }
            set
            {
                this.CODICEPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NUMEROPROTOCOLLO
        {
            get
            {
                return this.NUMEROPROTOCOLLOField;
            }
            set
            {
                this.NUMEROPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DATAPROTOCOLLO
        {
            get
            {
                return this.DATAPROTOCOLLOField;
            }
            set
            {
                this.DATAPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CODICEALLEGATO
        {
            get
            {
                return this.CODICEALLEGATOField;
            }
            set
            {
                this.CODICEALLEGATOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ANAG_MULTI", Namespace = "http://tempuri.org/")]
    public partial class ANAG_MULTI : object
    {

        private MULTI_ANAG[] DATOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_ANAG[] DATO
        {
            get
            {
                return this.DATOField;
            }
            set
            {
                this.DATOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_ANAG", Namespace = "http://tempuri.org/")]
    public partial class MULTI_ANAG : object
    {

        private string NOMINATIVOField;

        private string INDIRIZZOField;

        private string CITTAField;

        private string CAPField;

        private string PROVINCIAField;

        private string AZIENDAField;

        private string CODICEUNIVOCOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NOMINATIVO
        {
            get
            {
                return this.NOMINATIVOField;
            }
            set
            {
                this.NOMINATIVOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string INDIRIZZO
        {
            get
            {
                return this.INDIRIZZOField;
            }
            set
            {
                this.INDIRIZZOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CITTA
        {
            get
            {
                return this.CITTAField;
            }
            set
            {
                this.CITTAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CAP
        {
            get
            {
                return this.CAPField;
            }
            set
            {
                this.CAPField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string PROVINCIA
        {
            get
            {
                return this.PROVINCIAField;
            }
            set
            {
                this.PROVINCIAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string AZIENDA
        {
            get
            {
                return this.AZIENDAField;
            }
            set
            {
                this.AZIENDAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string CODICEUNIVOCO
        {
            get
            {
                return this.CODICEUNIVOCOField;
            }
            set
            {
                this.CODICEUNIVOCOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PROTOCOLLOEX_CREATO", Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLOEX_CREATO : object
    {

        private string CODICEPROTOCOLLOField;

        private string NUMEROPROTOCOLLOField;

        private string DATAPROTOCOLLOField;

        private string ESITOField;

        private string CODICEALLEGATOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICEPROTOCOLLO
        {
            get
            {
                return this.CODICEPROTOCOLLOField;
            }
            set
            {
                this.CODICEPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string NUMEROPROTOCOLLO
        {
            get
            {
                return this.NUMEROPROTOCOLLOField;
            }
            set
            {
                this.NUMEROPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DATAPROTOCOLLO
        {
            get
            {
                return this.DATAPROTOCOLLOField;
            }
            set
            {
                this.DATAPROTOCOLLOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CODICEALLEGATO
        {
            get
            {
                return this.CODICEALLEGATOField;
            }
            set
            {
                this.CODICEALLEGATOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PROTOCOLLOTRASMISSIONEResult", Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLOTRASMISSIONEResult : object
    {

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "INVIAPROTOCOLLOVIAPECResult", Namespace = "http://tempuri.org/")]
    public partial class INVIAPROTOCOLLOVIAPECResult : object
    {

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_TITOLARIO", Namespace = "http://tempuri.org/")]
    public partial class MULTI_TITOLARIO : object
    {

        private TITOLARIO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public TITOLARIO_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TITOLARIO_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class TITOLARIO_DOCUMENTO_TROVATO : object
    {

        private string CODICETITOLARIOField;

        private string DESCRIZIONEField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CODICETITOLARIO
        {
            get
            {
                return this.CODICETITOLARIOField;
            }
            set
            {
                this.CODICETITOLARIOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MULTI_UNITAORGANIZZATIVA", Namespace = "http://tempuri.org/")]
    public partial class MULTI_UNITAORGANIZZATIVA : object
    {

        private UNITAORGANIZZATIVA_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public UNITAORGANIZZATIVA_DOCUMENTO_TROVATO[] ELENCO_DOCUMENTI
        {
            get
            {
                return this.ELENCO_DOCUMENTIField;
            }
            set
            {
                this.ELENCO_DOCUMENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "UNITAORGANIZZATIVA_DOCUMENTO_TROVATO", Namespace = "http://tempuri.org/")]
    public partial class UNITAORGANIZZATIVA_DOCUMENTO_TROVATO : object
    {

        private string ACRONIMOField;

        private string DESCRIZIONEField;

        private string TIPOField;

        private string RESPONSABILEField;

        private string STATOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ACRONIMO
        {
            get
            {
                return this.ACRONIMOField;
            }
            set
            {
                this.ACRONIMOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TIPO
        {
            get
            {
                return this.TIPOField;
            }
            set
            {
                this.TIPOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string RESPONSABILE
        {
            get
            {
                return this.RESPONSABILEField;
            }
            set
            {
                this.RESPONSABILEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string STATO
        {
            get
            {
                return this.STATOField;
            }
            set
            {
                this.STATOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "INDICE_UNITAORGANIZZATIVA", Namespace = "http://tempuri.org/")]
    public partial class INDICE_UNITAORGANIZZATIVA : object
    {

        private UNITAORGANIZZATIVA_DOCUMENTO[] DATIField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public UNITAORGANIZZATIVA_DOCUMENTO[] DATI
        {
            get
            {
                return this.DATIField;
            }
            set
            {
                this.DATIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "UNITAORGANIZZATIVA_DOCUMENTO", Namespace = "http://tempuri.org/")]
    public partial class UNITAORGANIZZATIVA_DOCUMENTO : object
    {

        private string ACRONIMOField;

        private MULTI_VALORE[] AREAField;

        private MULTI_VALORE[] COMPONENTIField;

        private string DESCRIZIONEField;

        private MULTI_VALORE[] DIREZIONEField;

        private string FACENTEFUNZIONEField;

        private MULTI_VALORE[] LIVELLOSUPERIOREField;

        private string RESPONSABILEField;

        private MULTI_VALORE[] SEGRETERIAField;

        private string STATOField;

        private string TIPOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ACRONIMO
        {
            get
            {
                return this.ACRONIMOField;
            }
            set
            {
                this.ACRONIMOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] AREA
        {
            get
            {
                return this.AREAField;
            }
            set
            {
                this.AREAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] COMPONENTI
        {
            get
            {
                return this.COMPONENTIField;
            }
            set
            {
                this.COMPONENTIField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string DESCRIZIONE
        {
            get
            {
                return this.DESCRIZIONEField;
            }
            set
            {
                this.DESCRIZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] DIREZIONE
        {
            get
            {
                return this.DIREZIONEField;
            }
            set
            {
                this.DIREZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string FACENTEFUNZIONE
        {
            get
            {
                return this.FACENTEFUNZIONEField;
            }
            set
            {
                this.FACENTEFUNZIONEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] LIVELLOSUPERIORE
        {
            get
            {
                return this.LIVELLOSUPERIOREField;
            }
            set
            {
                this.LIVELLOSUPERIOREField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string RESPONSABILE
        {
            get
            {
                return this.RESPONSABILEField;
            }
            set
            {
                this.RESPONSABILEField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] SEGRETERIA
        {
            get
            {
                return this.SEGRETERIAField;
            }
            set
            {
                this.SEGRETERIAField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string STATO
        {
            get
            {
                return this.STATOField;
            }
            set
            {
                this.STATOField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string TIPO
        {
            get
            {
                return this.TIPOField;
            }
            set
            {
                this.TIPOField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "RISPOSTA_MULTI_VALORE", Namespace = "http://tempuri.org/")]
    public partial class RISPOSTA_MULTI_VALORE : object
    {

        private MULTI_VALORE[] VALOREField;

        private string ESITOField;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public MULTI_VALORE[] VALORE
        {
            get
            {
                return this.VALOREField;
            }
            set
            {
                this.VALOREField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ESITO
        {
            get
            {
                return this.ESITOField;
            }
            set
            {
                this.ESITOField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "SIGED_WSSoap")]
    public interface ISIGED_WSSoap
    {

        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento IDSESSION dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RICERCAANAGRAFICHE", ReplyAction = "*")]
        RICERCAANAGRAFICHEResponse RICERCAANAGRAFICHE(RICERCAANAGRAFICHERequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento IDSESSION dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/INDICEANAGRAFICA", ReplyAction = "*")]
        INDICEANAGRAFICAResponse INDICEANAGRAFICA(INDICEANAGRAFICARequest request);

 
        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAANAGRAFICA", ReplyAction = "*")]
        CREAANAGRAFICAResponse CREAANAGRAFICA(CREAANAGRAFICARequest request);

 
        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RICERCAFASCICOLI", ReplyAction = "*")]
        RICERCAFASCICOLIResponse RICERCAFASCICOLI(RICERCAFASCICOLIRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/INDICEFASCICOLO", ReplyAction = "*")]
        INDICEFASCICOLOResponse INDICEFASCICOLO(INDICEFASCICOLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAFASCICOLO", ReplyAction = "*")]
        CREAFASCICOLOResponse CREAFASCICOLO(CREAFASCICOLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREASOTTOFASCICOLO", ReplyAction = "*")]
        CREASOTTOFASCICOLOResponse CREASOTTOFASCICOLO(CREASOTTOFASCICOLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAFASCICOLOEXPRESS", ReplyAction = "*")]
        CREAFASCICOLOEXPRESSResponse CREAFASCICOLOEXPRESS(CREAFASCICOLOEXPRESSRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREASOTTOFASCICOLOEXPRESS", ReplyAction = "*")]
        CREASOTTOFASCICOLOEXPRESSResponse CREASOTTOFASCICOLOEXPRESS(CREASOTTOFASCICOLOEXPRESSRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAFASCICOLOMULTIPLO", ReplyAction = "*")]
        CREAFASCICOLOMULTIPLOResponse CREAFASCICOLOMULTIPLO(CREAFASCICOLOMULTIPLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREACOLLEGAMENTOFASCICOLO", ReplyAction = "*")]
        CREACOLLEGAMENTOFASCICOLOResponse CREACOLLEGAMENTOFASCICOLO(CREACOLLEGAMENTOFASCICOLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREADOCUMENTOINTERNO", ReplyAction = "*")]
        CREADOCUMENTOINTERNOResponse CREADOCUMENTOINTERNO(CREADOCUMENTOINTERNORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RESTITUISCIDOCUMENTOINTERNO", ReplyAction = "*")]
        RESTITUISCIDOCUMENTOINTERNOResponse RESTITUISCIDOCUMENTOINTERNO(RESTITUISCIDOCUMENTOINTERNORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RESTITUISCILINKFASCICOLO", ReplyAction = "*")]
        RESTITUISCILINKFASCICOLOResponse RESTITUISCILINKFASCICOLO(RESTITUISCILINKFASCICOLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RICERCAPROTOCOLLI", ReplyAction = "*")]
        RICERCAPROTOCOLLIResponse RICERCAPROTOCOLLI(RICERCAPROTOCOLLIRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/INDICEPROTOCOLLO", ReplyAction = "*")]
        INDICEPROTOCOLLOResponse INDICEPROTOCOLLO(INDICEPROTOCOLLORequest request);

        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAPROTOCOLLO", ReplyAction = "*")]
        CREAPROTOCOLLOResponse CREAPROTOCOLLO(CREAPROTOCOLLORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAPROTOCOLLOEXPRESS", ReplyAction = "*")]
        CREAPROTOCOLLOEXPRESSResponse CREAPROTOCOLLOEXPRESS(CREAPROTOCOLLOEXPRESSRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ANNULLAPROTOCOLLO", ReplyAction = "*")]
        ANNULLAPROTOCOLLOResponse ANNULLAPROTOCOLLO(ANNULLAPROTOCOLLORequest request);

        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CREAALLEGATO", ReplyAction = "*")]
        CREAALLEGATOResponse CREAALLEGATO(CREAALLEGATORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RESTITUISCIALLEGATO", ReplyAction = "*")]
        RESTITUISCIALLEGATOResponse RESTITUISCIALLEGATO(RESTITUISCIALLEGATORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CANCELLAALLEGATO", ReplyAction = "*")]
        CANCELLAALLEGATOResponse CANCELLAALLEGATO(CANCELLAALLEGATORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/PROTOCOLLOASSEGNAZIONE", ReplyAction = "*")]
        PROTOCOLLOASSEGNAZIONEResponse PROTOCOLLOASSEGNAZIONE(PROTOCOLLOASSEGNAZIONERequest request);

 
        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/PROTOCOLLOTRASMISSIONE", ReplyAction = "*")]
        PROTOCOLLOTRASMISSIONEResponse PROTOCOLLOTRASMISSIONE(PROTOCOLLOTRASMISSIONERequest request);

 
        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/INVIAPROTOCOLLOVIAPEC", ReplyAction = "*")]
        INVIAPROTOCOLLOVIAPECResponse INVIAPROTOCOLLOVIAPEC(INVIAPROTOCOLLOVIAPECRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RICERCATITOLARIO", ReplyAction = "*")]
        RICERCATITOLARIOResponse RICERCATITOLARIO(RICERCATITOLARIORequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/RICERCAUNITAORGANIZZATIVE", ReplyAction = "*")]
        RICERCAUNITAORGANIZZATIVEResponse RICERCAUNITAORGANIZZATIVE(RICERCAUNITAORGANIZZATIVERequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/INDICEUNITAORGANIZZATIVA", ReplyAction = "*")]
        INDICEUNITAORGANIZZATIVAResponse INDICEUNITAORGANIZZATIVA(INDICEUNITAORGANIZZATIVARequest request);

         // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/LISTATIPODOCUMENTO", ReplyAction = "*")]
        LISTATIPODOCUMENTOResponse LISTATIPODOCUMENTO(LISTATIPODOCUMENTORequest request);

 
        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento SESSIONE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/LISTACATEGORIEFASCICOLI", ReplyAction = "*")]
        LISTACATEGORIEFASCICOLIResponse LISTACATEGORIEFASCICOLI(LISTACATEGORIEFASCICOLIRequest request);


        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento VALORESINGLE dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CONVERTIINMULTIVALORE", ReplyAction = "*")]
        CONVERTIINMULTIVALOREResponse CONVERTIINMULTIVALORE(CONVERTIINMULTIVALORERequest request);

    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAANAGRAFICHERequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAANAGRAFICHE", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAANAGRAFICHERequestBody Body;

        public RICERCAANAGRAFICHERequest()
        {
        }

        public RICERCAANAGRAFICHERequest(RICERCAANAGRAFICHERequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAANAGRAFICHERequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string IDSESSION;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEANAGRAFICA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string NOMINATIVO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CITTA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string PROVINCIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string CAP;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string CODICEFISCALE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string CODICEUNIVOCO;

        public RICERCAANAGRAFICHERequestBody()
        {
        }

        public RICERCAANAGRAFICHERequestBody(string IDSESSION, string CODICEANAGRAFICA, string NOMINATIVO, string CITTA, string PROVINCIA, string CAP, string CODICEFISCALE, string CODICEUNIVOCO)
        {
            this.IDSESSION = IDSESSION;
            this.CODICEANAGRAFICA = CODICEANAGRAFICA;
            this.NOMINATIVO = NOMINATIVO;
            this.CITTA = CITTA;
            this.PROVINCIA = PROVINCIA;
            this.CAP = CAP;
            this.CODICEFISCALE = CODICEFISCALE;
            this.CODICEUNIVOCO = CODICEUNIVOCO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAANAGRAFICHEResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAANAGRAFICHEResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAANAGRAFICHEResponseBody Body;

        public RICERCAANAGRAFICHEResponse()
        {
        }

        public RICERCAANAGRAFICHEResponse(RICERCAANAGRAFICHEResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAANAGRAFICHEResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public MULTI_ANAGRAFICA RICERCAANAGRAFICHEResult;

        public RICERCAANAGRAFICHEResponseBody()
        {
        }

        public RICERCAANAGRAFICHEResponseBody(MULTI_ANAGRAFICA RICERCAANAGRAFICHEResult)
        {
            this.RICERCAANAGRAFICHEResult = RICERCAANAGRAFICHEResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEANAGRAFICARequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEANAGRAFICA", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEANAGRAFICARequestBody Body;

        public INDICEANAGRAFICARequest()
        {
        }

        public INDICEANAGRAFICARequest(INDICEANAGRAFICARequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEANAGRAFICARequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string IDSESSION;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEANAGRAFICA;

        public INDICEANAGRAFICARequestBody()
        {
        }

        public INDICEANAGRAFICARequestBody(string IDSESSION, string CODICEANAGRAFICA)
        {
            this.IDSESSION = IDSESSION;
            this.CODICEANAGRAFICA = CODICEANAGRAFICA;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEANAGRAFICAResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEANAGRAFICAResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEANAGRAFICAResponseBody Body;

        public INDICEANAGRAFICAResponse()
        {
        }

        public INDICEANAGRAFICAResponse(INDICEANAGRAFICAResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEANAGRAFICAResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INDICE_ANAGRAFICA INDICEANAGRAFICAResult;

        public INDICEANAGRAFICAResponseBody()
        {
        }

        public INDICEANAGRAFICAResponseBody(INDICE_ANAGRAFICA INDICEANAGRAFICAResult)
        {
            this.INDICEANAGRAFICAResult = INDICEANAGRAFICAResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAANAGRAFICARequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAANAGRAFICA", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAANAGRAFICARequestBody Body;

        public CREAANAGRAFICARequest()
        {
        }

        public CREAANAGRAFICARequest(CREAANAGRAFICARequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAANAGRAFICARequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string NOMINATIVO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string TIPOLOGIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string INDIRIZZO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CITTA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string CAP;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string PROVINCIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string FAX;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string EMAIL;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string AZIENDA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string LUOGONASCITA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string DATANASCITA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string CODICEFISCALE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string PARTITAIVA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string CODICEUNIVOCO;

        public CREAANAGRAFICARequestBody()
        {
        }

        public CREAANAGRAFICARequestBody(string SESSIONE, string NOMINATIVO, string TIPOLOGIA, string INDIRIZZO, string CITTA, string CAP, string PROVINCIA, string FAX, string EMAIL, string AZIENDA, string LUOGONASCITA, string DATANASCITA, string CODICEFISCALE, string PARTITAIVA, string CODICEUNIVOCO)
        {
            this.SESSIONE = SESSIONE;
            this.NOMINATIVO = NOMINATIVO;
            this.TIPOLOGIA = TIPOLOGIA;
            this.INDIRIZZO = INDIRIZZO;
            this.CITTA = CITTA;
            this.CAP = CAP;
            this.PROVINCIA = PROVINCIA;
            this.FAX = FAX;
            this.EMAIL = EMAIL;
            this.AZIENDA = AZIENDA;
            this.LUOGONASCITA = LUOGONASCITA;
            this.DATANASCITA = DATANASCITA;
            this.CODICEFISCALE = CODICEFISCALE;
            this.PARTITAIVA = PARTITAIVA;
            this.CODICEUNIVOCO = CODICEUNIVOCO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAANAGRAFICAResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAANAGRAFICAResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAANAGRAFICAResponseBody Body;

        public CREAANAGRAFICAResponse()
        {
        }

        public CREAANAGRAFICAResponse(CREAANAGRAFICAResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAANAGRAFICAResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CREAANAGRAFICAResult;

        public CREAANAGRAFICAResponseBody()
        {
        }

        public CREAANAGRAFICAResponseBody(string CREAANAGRAFICAResult)
        {
            this.CREAANAGRAFICAResult = CREAANAGRAFICAResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAFASCICOLIRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAFASCICOLI", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAFASCICOLIRequestBody Body;

        public RICERCAFASCICOLIRequest()
        {
        }

        public RICERCAFASCICOLIRequest(RICERCAFASCICOLIRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAFASCICOLIRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEFASCICOLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string RIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string TITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string NUMEROFASCICOLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string STATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string CATEGORIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string DATAPRATICA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string STRUTTURA;

        public RICERCAFASCICOLIRequestBody()
        {
        }

        public RICERCAFASCICOLIRequestBody(string SESSIONE, string CODICEFASCICOLO, string DESCRIZIONE, string RIFERIMENTO, string TITOLARIO, string NUMEROFASCICOLO, string STATO, string CATEGORIA, string DATAPRATICA, string STRUTTURA)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
            this.DESCRIZIONE = DESCRIZIONE;
            this.RIFERIMENTO = RIFERIMENTO;
            this.TITOLARIO = TITOLARIO;
            this.NUMEROFASCICOLO = NUMEROFASCICOLO;
            this.STATO = STATO;
            this.CATEGORIA = CATEGORIA;
            this.DATAPRATICA = DATAPRATICA;
            this.STRUTTURA = STRUTTURA;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAFASCICOLIResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAFASCICOLIResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAFASCICOLIResponseBody Body;

        public RICERCAFASCICOLIResponse()
        {
        }

        public RICERCAFASCICOLIResponse(RICERCAFASCICOLIResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAFASCICOLIResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public MULTI_FASCICOLO RICERCAFASCICOLIResult;

        public RICERCAFASCICOLIResponseBody()
        {
        }

        public RICERCAFASCICOLIResponseBody(MULTI_FASCICOLO RICERCAFASCICOLIResult)
        {
            this.RICERCAFASCICOLIResult = RICERCAFASCICOLIResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEFASCICOLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEFASCICOLO", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEFASCICOLORequestBody Body;

        public INDICEFASCICOLORequest()
        {
        }

        public INDICEFASCICOLORequest(INDICEFASCICOLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEFASCICOLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEFASCICOLO;

        public INDICEFASCICOLORequestBody()
        {
        }

        public INDICEFASCICOLORequestBody(string SESSIONE, string CODICEFASCICOLO)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEFASCICOLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEFASCICOLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEFASCICOLOResponseBody Body;

        public INDICEFASCICOLOResponse()
        {
        }

        public INDICEFASCICOLOResponse(INDICEFASCICOLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEFASCICOLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INDICE_FASCICOLO INDICEFASCICOLOResult;

        public INDICEFASCICOLOResponseBody()
        {
        }

        public INDICEFASCICOLOResponseBody(INDICE_FASCICOLO INDICEFASCICOLOResult)
        {
            this.INDICEFASCICOLOResult = INDICEFASCICOLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAFASCICOLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAFASCICOLO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAFASCICOLORequestBody Body;

        public CREAFASCICOLORequest()
        {
        }

        public CREAFASCICOLORequest(CREAFASCICOLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAFASCICOLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string RIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string STATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string DATAAPERTURA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string CATEGORIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string CODICEDEFAULT;

        public CREAFASCICOLORequestBody()
        {
        }

        public CREAFASCICOLORequestBody(string SESSIONE, string DESCRIZIONE, string RIFERIMENTO, string CODICETITOLARIO, string STATO, string DATAAPERTURA, string UNITAORGANIZZATIVARESPONSABILE, string CATEGORIA, string CODICEDEFAULT)
        {
            this.SESSIONE = SESSIONE;
            this.DESCRIZIONE = DESCRIZIONE;
            this.RIFERIMENTO = RIFERIMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.STATO = STATO;
            this.DATAAPERTURA = DATAAPERTURA;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.CATEGORIA = CATEGORIA;
            this.CODICEDEFAULT = CODICEDEFAULT;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAFASCICOLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAFASCICOLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAFASCICOLOResponseBody Body;

        public CREAFASCICOLOResponse()
        {
        }

        public CREAFASCICOLOResponse(CREAFASCICOLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAFASCICOLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public FASCICOLO_CREATO CREAFASCICOLOResult;

        public CREAFASCICOLOResponseBody()
        {
        }

        public CREAFASCICOLOResponseBody(FASCICOLO_CREATO CREAFASCICOLOResult)
        {
            this.CREAFASCICOLOResult = CREAFASCICOLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREASOTTOFASCICOLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREASOTTOFASCICOLO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREASOTTOFASCICOLORequestBody Body;

        public CREASOTTOFASCICOLORequest()
        {
        }

        public CREASOTTOFASCICOLORequest(CREASOTTOFASCICOLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREASOTTOFASCICOLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string RIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CODICEFASCICOLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string STATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string DATAAPERTURA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string CATEGORIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string CODICEDEFAULT;

        public CREASOTTOFASCICOLORequestBody()
        {
        }

        public CREASOTTOFASCICOLORequestBody(string SESSIONE, string DESCRIZIONE, string RIFERIMENTO, string CODICETITOLARIO, string CODICEFASCICOLO, string STATO, string DATAAPERTURA, string UNITAORGANIZZATIVARESPONSABILE, string CATEGORIA, string CODICEDEFAULT)
        {
            this.SESSIONE = SESSIONE;
            this.DESCRIZIONE = DESCRIZIONE;
            this.RIFERIMENTO = RIFERIMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
            this.STATO = STATO;
            this.DATAAPERTURA = DATAAPERTURA;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.CATEGORIA = CATEGORIA;
            this.CODICEDEFAULT = CODICEDEFAULT;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREASOTTOFASCICOLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREASOTTOFASCICOLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREASOTTOFASCICOLOResponseBody Body;

        public CREASOTTOFASCICOLOResponse()
        {
        }

        public CREASOTTOFASCICOLOResponse(CREASOTTOFASCICOLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREASOTTOFASCICOLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public SOTTOFASCICOLO_CREATO CREASOTTOFASCICOLOResult;

        public CREASOTTOFASCICOLOResponseBody()
        {
        }

        public CREASOTTOFASCICOLOResponseBody(SOTTOFASCICOLO_CREATO CREASOTTOFASCICOLOResult)
        {
            this.CREASOTTOFASCICOLOResult = CREASOTTOFASCICOLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAFASCICOLOEXPRESSRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAFASCICOLOEXPRESS", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAFASCICOLOEXPRESSRequestBody Body;

        public CREAFASCICOLOEXPRESSRequest()
        {
        }

        public CREAFASCICOLOEXPRESSRequest(CREAFASCICOLOEXPRESSRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAFASCICOLOEXPRESSRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string RIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string STATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string DATAAPERTURA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string CATEGORIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string DESCRIZIONEDOCINT;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string NOMEFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string BASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string PATHFULLFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string CODICEDEFAULT;

        public CREAFASCICOLOEXPRESSRequestBody()
        {
        }

        public CREAFASCICOLOEXPRESSRequestBody(string SESSIONE, string CODICEPROTOCOLLO, string DESCRIZIONE, string RIFERIMENTO, string CODICETITOLARIO, string STATO, string DATAAPERTURA, string UNITAORGANIZZATIVARESPONSABILE, string CATEGORIA, string DESCRIZIONEDOCINT, string NOMEFILE, string BASE64, string PATHFULLFILE, string CODICEDEFAULT)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
            this.DESCRIZIONE = DESCRIZIONE;
            this.RIFERIMENTO = RIFERIMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.STATO = STATO;
            this.DATAAPERTURA = DATAAPERTURA;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.CATEGORIA = CATEGORIA;
            this.DESCRIZIONEDOCINT = DESCRIZIONEDOCINT;
            this.NOMEFILE = NOMEFILE;
            this.BASE64 = BASE64;
            this.PATHFULLFILE = PATHFULLFILE;
            this.CODICEDEFAULT = CODICEDEFAULT;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAFASCICOLOEXPRESSResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAFASCICOLOEXPRESSResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAFASCICOLOEXPRESSResponseBody Body;

        public CREAFASCICOLOEXPRESSResponse()
        {
        }

        public CREAFASCICOLOEXPRESSResponse(CREAFASCICOLOEXPRESSResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAFASCICOLOEXPRESSResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public FASCICOLOEX_CREATO CREAFASCICOLOEXPRESSResult;

        public CREAFASCICOLOEXPRESSResponseBody()
        {
        }

        public CREAFASCICOLOEXPRESSResponseBody(FASCICOLOEX_CREATO CREAFASCICOLOEXPRESSResult)
        {
            this.CREAFASCICOLOEXPRESSResult = CREAFASCICOLOEXPRESSResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREASOTTOFASCICOLOEXPRESSRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREASOTTOFASCICOLOEXPRESS", Namespace = "http://tempuri.org/", Order = 0)]
        public CREASOTTOFASCICOLOEXPRESSRequestBody Body;

        public CREASOTTOFASCICOLOEXPRESSRequest()
        {
        }

        public CREASOTTOFASCICOLOEXPRESSRequest(CREASOTTOFASCICOLOEXPRESSRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREASOTTOFASCICOLOEXPRESSRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string RIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string CODICEFASCICOLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string STATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string DATAAPERTURA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string CATEGORIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string DESCRIZIONEDOCINT;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string NOMEFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string BASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string PATHFULLFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string CODICEDEFAULT;

        public CREASOTTOFASCICOLOEXPRESSRequestBody()
        {
        }

        public CREASOTTOFASCICOLOEXPRESSRequestBody(string SESSIONE, string CODICEPROTOCOLLO, string DESCRIZIONE, string RIFERIMENTO, string CODICETITOLARIO, string CODICEFASCICOLO, string STATO, string DATAAPERTURA, string UNITAORGANIZZATIVARESPONSABILE, string CATEGORIA, string DESCRIZIONEDOCINT, string NOMEFILE, string BASE64, string PATHFULLFILE, string CODICEDEFAULT)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
            this.DESCRIZIONE = DESCRIZIONE;
            this.RIFERIMENTO = RIFERIMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
            this.STATO = STATO;
            this.DATAAPERTURA = DATAAPERTURA;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.CATEGORIA = CATEGORIA;
            this.DESCRIZIONEDOCINT = DESCRIZIONEDOCINT;
            this.NOMEFILE = NOMEFILE;
            this.BASE64 = BASE64;
            this.PATHFULLFILE = PATHFULLFILE;
            this.CODICEDEFAULT = CODICEDEFAULT;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREASOTTOFASCICOLOEXPRESSResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREASOTTOFASCICOLOEXPRESSResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREASOTTOFASCICOLOEXPRESSResponseBody Body;

        public CREASOTTOFASCICOLOEXPRESSResponse()
        {
        }

        public CREASOTTOFASCICOLOEXPRESSResponse(CREASOTTOFASCICOLOEXPRESSResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREASOTTOFASCICOLOEXPRESSResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public SOTTOFASCICOLOEX_CREATO CREASOTTOFASCICOLOEXPRESSResult;

        public CREASOTTOFASCICOLOEXPRESSResponseBody()
        {
        }

        public CREASOTTOFASCICOLOEXPRESSResponseBody(SOTTOFASCICOLOEX_CREATO CREASOTTOFASCICOLOEXPRESSResult)
        {
            this.CREASOTTOFASCICOLOEXPRESSResult = CREASOTTOFASCICOLOEXPRESSResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAFASCICOLOMULTIPLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAFASCICOLOMULTIPLO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAFASCICOLOMULTIPLORequestBody Body;

        public CREAFASCICOLOMULTIPLORequest()
        {
        }

        public CREAFASCICOLOMULTIPLORequest(CREAFASCICOLOMULTIPLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAFASCICOLOMULTIPLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public DATO_MULTI DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string RIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string STATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string DATAAPERTURA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string CATEGORIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string CODICEDEFAULT;

        public CREAFASCICOLOMULTIPLORequestBody()
        {
        }

        public CREAFASCICOLOMULTIPLORequestBody(string SESSIONE, DATO_MULTI DESCRIZIONE, string RIFERIMENTO, string CODICETITOLARIO, string STATO, string DATAAPERTURA, string UNITAORGANIZZATIVARESPONSABILE, string CATEGORIA, string CODICEDEFAULT)
        {
            this.SESSIONE = SESSIONE;
            this.DESCRIZIONE = DESCRIZIONE;
            this.RIFERIMENTO = RIFERIMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.STATO = STATO;
            this.DATAAPERTURA = DATAAPERTURA;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.CATEGORIA = CATEGORIA;
            this.CODICEDEFAULT = CODICEDEFAULT;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAFASCICOLOMULTIPLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAFASCICOLOMULTIPLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAFASCICOLOMULTIPLOResponseBody Body;

        public CREAFASCICOLOMULTIPLOResponse()
        {
        }

        public CREAFASCICOLOMULTIPLOResponse(CREAFASCICOLOMULTIPLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAFASCICOLOMULTIPLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public MULTI_FASCICOLO_CREATO CREAFASCICOLOMULTIPLOResult;

        public CREAFASCICOLOMULTIPLOResponseBody()
        {
        }

        public CREAFASCICOLOMULTIPLOResponseBody(MULTI_FASCICOLO_CREATO CREAFASCICOLOMULTIPLOResult)
        {
            this.CREAFASCICOLOMULTIPLOResult = CREAFASCICOLOMULTIPLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREACOLLEGAMENTOFASCICOLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREACOLLEGAMENTOFASCICOLO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREACOLLEGAMENTOFASCICOLORequestBody Body;

        public CREACOLLEGAMENTOFASCICOLORequest()
        {
        }

        public CREACOLLEGAMENTOFASCICOLORequest(CREACOLLEGAMENTOFASCICOLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREACOLLEGAMENTOFASCICOLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string TIPOCOLLEGAMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CODICEFASCICOLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CODICEPROTOCOLLOCOLLEGATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CODICEFASCICOLOCOLLEGATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string FLAGAPERTURA;

        public CREACOLLEGAMENTOFASCICOLORequestBody()
        {
        }

        public CREACOLLEGAMENTOFASCICOLORequestBody(string SESSIONE, string TIPOCOLLEGAMENTO, string CODICEFASCICOLO, string CODICEPROTOCOLLOCOLLEGATO, string CODICEFASCICOLOCOLLEGATO, string FLAGAPERTURA)
        {
            this.SESSIONE = SESSIONE;
            this.TIPOCOLLEGAMENTO = TIPOCOLLEGAMENTO;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
            this.CODICEPROTOCOLLOCOLLEGATO = CODICEPROTOCOLLOCOLLEGATO;
            this.CODICEFASCICOLOCOLLEGATO = CODICEFASCICOLOCOLLEGATO;
            this.FLAGAPERTURA = FLAGAPERTURA;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREACOLLEGAMENTOFASCICOLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREACOLLEGAMENTOFASCICOLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREACOLLEGAMENTOFASCICOLOResponseBody Body;

        public CREACOLLEGAMENTOFASCICOLOResponse()
        {
        }

        public CREACOLLEGAMENTOFASCICOLOResponse(CREACOLLEGAMENTOFASCICOLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREACOLLEGAMENTOFASCICOLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CREACOLLEGAMENTOFASCICOLOResult;

        public CREACOLLEGAMENTOFASCICOLOResponseBody()
        {
        }

        public CREACOLLEGAMENTOFASCICOLOResponseBody(string CREACOLLEGAMENTOFASCICOLOResult)
        {
            this.CREACOLLEGAMENTOFASCICOLOResult = CREACOLLEGAMENTOFASCICOLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREADOCUMENTOINTERNORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREADOCUMENTOINTERNO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREADOCUMENTOINTERNORequestBody Body;

        public CREADOCUMENTOINTERNORequest()
        {
        }

        public CREADOCUMENTOINTERNORequest(CREADOCUMENTOINTERNORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREADOCUMENTOINTERNORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEFASCICOLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string NOMEFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string BASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string PATHFULLFILE;

        public CREADOCUMENTOINTERNORequestBody()
        {
        }

        public CREADOCUMENTOINTERNORequestBody(string SESSIONE, string CODICEFASCICOLO, string DESCRIZIONE, string NOMEFILE, string BASE64, string PATHFULLFILE)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
            this.DESCRIZIONE = DESCRIZIONE;
            this.NOMEFILE = NOMEFILE;
            this.BASE64 = BASE64;
            this.PATHFULLFILE = PATHFULLFILE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREADOCUMENTOINTERNOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREADOCUMENTOINTERNOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREADOCUMENTOINTERNOResponseBody Body;

        public CREADOCUMENTOINTERNOResponse()
        {
        }

        public CREADOCUMENTOINTERNOResponse(CREADOCUMENTOINTERNOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREADOCUMENTOINTERNOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CREADOCUMENTOINTERNOResult;

        public CREADOCUMENTOINTERNOResponseBody()
        {
        }

        public CREADOCUMENTOINTERNOResponseBody(string CREADOCUMENTOINTERNOResult)
        {
            this.CREADOCUMENTOINTERNOResult = CREADOCUMENTOINTERNOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RESTITUISCIDOCUMENTOINTERNORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RESTITUISCIDOCUMENTOINTERNO", Namespace = "http://tempuri.org/", Order = 0)]
        public RESTITUISCIDOCUMENTOINTERNORequestBody Body;

        public RESTITUISCIDOCUMENTOINTERNORequest()
        {
        }

        public RESTITUISCIDOCUMENTOINTERNORequest(RESTITUISCIDOCUMENTOINTERNORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RESTITUISCIDOCUMENTOINTERNORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEDOCUMENTOINTERNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string FLAGBASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string PATHFULLFILE;

        public RESTITUISCIDOCUMENTOINTERNORequestBody()
        {
        }

        public RESTITUISCIDOCUMENTOINTERNORequestBody(string SESSIONE, string CODICEDOCUMENTOINTERNO, string FLAGBASE64, string PATHFULLFILE)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEDOCUMENTOINTERNO = CODICEDOCUMENTOINTERNO;
            this.FLAGBASE64 = FLAGBASE64;
            this.PATHFULLFILE = PATHFULLFILE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RESTITUISCIDOCUMENTOINTERNOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RESTITUISCIDOCUMENTOINTERNOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RESTITUISCIDOCUMENTOINTERNOResponseBody Body;

        public RESTITUISCIDOCUMENTOINTERNOResponse()
        {
        }

        public RESTITUISCIDOCUMENTOINTERNOResponse(RESTITUISCIDOCUMENTOINTERNOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RESTITUISCIDOCUMENTOINTERNOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INDICE_ALLEGATO RESTITUISCIDOCUMENTOINTERNOResult;

        public RESTITUISCIDOCUMENTOINTERNOResponseBody()
        {
        }

        public RESTITUISCIDOCUMENTOINTERNOResponseBody(INDICE_ALLEGATO RESTITUISCIDOCUMENTOINTERNOResult)
        {
            this.RESTITUISCIDOCUMENTOINTERNOResult = RESTITUISCIDOCUMENTOINTERNOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RESTITUISCILINKFASCICOLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RESTITUISCILINKFASCICOLO", Namespace = "http://tempuri.org/", Order = 0)]
        public RESTITUISCILINKFASCICOLORequestBody Body;

        public RESTITUISCILINKFASCICOLORequest()
        {
        }

        public RESTITUISCILINKFASCICOLORequest(RESTITUISCILINKFASCICOLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RESTITUISCILINKFASCICOLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEFASCICOLO;

        public RESTITUISCILINKFASCICOLORequestBody()
        {
        }

        public RESTITUISCILINKFASCICOLORequestBody(string SESSIONE, string CODICEFASCICOLO)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEFASCICOLO = CODICEFASCICOLO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RESTITUISCILINKFASCICOLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RESTITUISCILINKFASCICOLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RESTITUISCILINKFASCICOLOResponseBody Body;

        public RESTITUISCILINKFASCICOLOResponse()
        {
        }

        public RESTITUISCILINKFASCICOLOResponse(RESTITUISCILINKFASCICOLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RESTITUISCILINKFASCICOLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string RESTITUISCILINKFASCICOLOResult;

        public RESTITUISCILINKFASCICOLOResponseBody()
        {
        }

        public RESTITUISCILINKFASCICOLOResponseBody(string RESTITUISCILINKFASCICOLOResult)
        {
            this.RESTITUISCILINKFASCICOLOResult = RESTITUISCILINKFASCICOLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAPROTOCOLLIRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAPROTOCOLLI", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAPROTOCOLLIRequestBody Body;

        public RICERCAPROTOCOLLIRequest()
        {
        }

        public RICERCAPROTOCOLLIRequest(RICERCAPROTOCOLLIRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAPROTOCOLLIRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ANNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string NUMEROPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string DATAPROTOCOLLAZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CORRISPONDENTE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string OGGETTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string TIPOPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string ESTREMIPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string PROTOCOLLORIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string STATO;

        public RICERCAPROTOCOLLIRequestBody()
        {
        }

        public RICERCAPROTOCOLLIRequestBody(string SESSIONE, string ANNO, string NUMEROPROTOCOLLO, string DATAPROTOCOLLAZIONE, string CORRISPONDENTE, string OGGETTO, string TIPOPROTOCOLLO, string ESTREMIPROTOCOLLO, string PROTOCOLLORIFERIMENTO, string STATO)
        {
            this.SESSIONE = SESSIONE;
            this.ANNO = ANNO;
            this.NUMEROPROTOCOLLO = NUMEROPROTOCOLLO;
            this.DATAPROTOCOLLAZIONE = DATAPROTOCOLLAZIONE;
            this.CORRISPONDENTE = CORRISPONDENTE;
            this.OGGETTO = OGGETTO;
            this.TIPOPROTOCOLLO = TIPOPROTOCOLLO;
            this.ESTREMIPROTOCOLLO = ESTREMIPROTOCOLLO;
            this.PROTOCOLLORIFERIMENTO = PROTOCOLLORIFERIMENTO;
            this.STATO = STATO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAPROTOCOLLIResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAPROTOCOLLIResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAPROTOCOLLIResponseBody Body;

        public RICERCAPROTOCOLLIResponse()
        {
        }

        public RICERCAPROTOCOLLIResponse(RICERCAPROTOCOLLIResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAPROTOCOLLIResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public MULTI_PROTOCOLLO RICERCAPROTOCOLLIResult;

        public RICERCAPROTOCOLLIResponseBody()
        {
        }

        public RICERCAPROTOCOLLIResponseBody(MULTI_PROTOCOLLO RICERCAPROTOCOLLIResult)
        {
            this.RICERCAPROTOCOLLIResult = RICERCAPROTOCOLLIResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEPROTOCOLLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEPROTOCOLLO", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEPROTOCOLLORequestBody Body;

        public INDICEPROTOCOLLORequest()
        {
        }

        public INDICEPROTOCOLLORequest(INDICEPROTOCOLLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEPROTOCOLLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        public INDICEPROTOCOLLORequestBody()
        {
        }

        public INDICEPROTOCOLLORequestBody(string SESSIONE, string CODICEPROTOCOLLO)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEPROTOCOLLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEPROTOCOLLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEPROTOCOLLOResponseBody Body;

        public INDICEPROTOCOLLOResponse()
        {
        }

        public INDICEPROTOCOLLOResponse(INDICEPROTOCOLLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEPROTOCOLLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INDICE_PROTOCOLLO INDICEPROTOCOLLOResult;

        public INDICEPROTOCOLLOResponseBody()
        {
        }

        public INDICEPROTOCOLLOResponseBody(INDICE_PROTOCOLLO INDICEPROTOCOLLOResult)
        {
            this.INDICEPROTOCOLLOResult = INDICEPROTOCOLLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAPROTOCOLLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAPROTOCOLLO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAPROTOCOLLORequestBody Body;

        public CREAPROTOCOLLORequest()
        {
        }

        public CREAPROTOCOLLORequest(CREAPROTOCOLLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAPROTOCOLLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ANNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string TIPOPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CODICECORRISPONDENTE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string OGGETTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string TIPODOCUMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string ESTREMI;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string DATAESTREMI;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string PROTOCOLLORIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string CODICEDEFAULT;

        public CREAPROTOCOLLORequestBody()
        {
        }

        public CREAPROTOCOLLORequestBody(string SESSIONE, string ANNO, string TIPOPROTOCOLLO, string CODICECORRISPONDENTE, string OGGETTO, string UNITAORGANIZZATIVARESPONSABILE, string TIPODOCUMENTO, string CODICETITOLARIO, string ESTREMI, string DATAESTREMI, string PROTOCOLLORIFERIMENTO, string CODICEDEFAULT)
        {
            this.SESSIONE = SESSIONE;
            this.ANNO = ANNO;
            this.TIPOPROTOCOLLO = TIPOPROTOCOLLO;
            this.CODICECORRISPONDENTE = CODICECORRISPONDENTE;
            this.OGGETTO = OGGETTO;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.TIPODOCUMENTO = TIPODOCUMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.ESTREMI = ESTREMI;
            this.DATAESTREMI = DATAESTREMI;
            this.PROTOCOLLORIFERIMENTO = PROTOCOLLORIFERIMENTO;
            this.CODICEDEFAULT = CODICEDEFAULT;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAPROTOCOLLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAPROTOCOLLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAPROTOCOLLOResponseBody Body;

        public CREAPROTOCOLLOResponse()
        {
        }

        public CREAPROTOCOLLOResponse(CREAPROTOCOLLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAPROTOCOLLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public PROTOCOLLO_CREATO CREAPROTOCOLLOResult;

        public CREAPROTOCOLLOResponseBody()
        {
        }

        public CREAPROTOCOLLOResponseBody(PROTOCOLLO_CREATO CREAPROTOCOLLOResult)
        {
            this.CREAPROTOCOLLOResult = CREAPROTOCOLLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAPROTOCOLLOEXPRESSRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAPROTOCOLLOEXPRESS", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAPROTOCOLLOEXPRESSRequestBody Body;

        public CREAPROTOCOLLOEXPRESSRequest()
        {
        }

        public CREAPROTOCOLLOEXPRESSRequest(CREAPROTOCOLLOEXPRESSRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAPROTOCOLLOEXPRESSRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ANNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string TIPOPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CODICEANAGRAFICA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string CORRISPONDENTENOMINATIVO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string CORRISPONDENTEINDIRIZZO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string CORRISPONDENTECITTA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string CORRISPONDENTECAP;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string CORRISPONDENTEPROVINCIA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string CORRISPONDENTEAZIENDA;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string CORRISPONDENTECODICEUNIVOCO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public ANAG_MULTI MULTIANAG;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string OGGETTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string UNITAORGANIZZATIVARESPONSABILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string TIPODOCUMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 15)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 16)]
        public string ESTREMI;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public string DATAESTREMI;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 18)]
        public string PROTOCOLLORIFERIMENTO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 19)]
        public string ALLEGATODESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 20)]
        public string ALLEGATONOMEFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 21)]
        public string ALLEGATOBASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 22)]
        public string ALLEGATOPATHFULLFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 23)]
        public string CODICEDEFAULT;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 24)]
        public string TIPOALLEGATO;

        public CREAPROTOCOLLOEXPRESSRequestBody()
        {
        }

        public CREAPROTOCOLLOEXPRESSRequestBody(
                    string SESSIONE,
                    string ANNO,
                    string TIPOPROTOCOLLO,
                    string CODICEANAGRAFICA,
                    string CORRISPONDENTENOMINATIVO,
                    string CORRISPONDENTEINDIRIZZO,
                    string CORRISPONDENTECITTA,
                    string CORRISPONDENTECAP,
                    string CORRISPONDENTEPROVINCIA,
                    string CORRISPONDENTEAZIENDA,
                    string CORRISPONDENTECODICEUNIVOCO,
                    ANAG_MULTI MULTIANAG,
                    string OGGETTO,
                    string UNITAORGANIZZATIVARESPONSABILE,
                    string TIPODOCUMENTO,
                    string CODICETITOLARIO,
                    string ESTREMI,
                    string DATAESTREMI,
                    string PROTOCOLLORIFERIMENTO,
                    string ALLEGATODESCRIZIONE,
                    string ALLEGATONOMEFILE,
                    string ALLEGATOBASE64,
                    string ALLEGATOPATHFULLFILE,
                    string CODICEDEFAULT,
                    string TIPOALLEGATO)
        {
            this.SESSIONE = SESSIONE;
            this.ANNO = ANNO;
            this.TIPOPROTOCOLLO = TIPOPROTOCOLLO;
            this.CODICEANAGRAFICA = CODICEANAGRAFICA;
            this.CORRISPONDENTENOMINATIVO = CORRISPONDENTENOMINATIVO;
            this.CORRISPONDENTEINDIRIZZO = CORRISPONDENTEINDIRIZZO;
            this.CORRISPONDENTECITTA = CORRISPONDENTECITTA;
            this.CORRISPONDENTECAP = CORRISPONDENTECAP;
            this.CORRISPONDENTEPROVINCIA = CORRISPONDENTEPROVINCIA;
            this.CORRISPONDENTEAZIENDA = CORRISPONDENTEAZIENDA;
            this.CORRISPONDENTECODICEUNIVOCO = CORRISPONDENTECODICEUNIVOCO;
            this.MULTIANAG = MULTIANAG;
            this.OGGETTO = OGGETTO;
            this.UNITAORGANIZZATIVARESPONSABILE = UNITAORGANIZZATIVARESPONSABILE;
            this.TIPODOCUMENTO = TIPODOCUMENTO;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.ESTREMI = ESTREMI;
            this.DATAESTREMI = DATAESTREMI;
            this.PROTOCOLLORIFERIMENTO = PROTOCOLLORIFERIMENTO;
            this.ALLEGATODESCRIZIONE = ALLEGATODESCRIZIONE;
            this.ALLEGATONOMEFILE = ALLEGATONOMEFILE;
            this.ALLEGATOBASE64 = ALLEGATOBASE64;
            this.ALLEGATOPATHFULLFILE = ALLEGATOPATHFULLFILE;
            this.CODICEDEFAULT = CODICEDEFAULT;
            this.TIPOALLEGATO = TIPOALLEGATO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAPROTOCOLLOEXPRESSResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAPROTOCOLLOEXPRESSResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAPROTOCOLLOEXPRESSResponseBody Body;

        public CREAPROTOCOLLOEXPRESSResponse()
        {
        }

        public CREAPROTOCOLLOEXPRESSResponse(CREAPROTOCOLLOEXPRESSResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAPROTOCOLLOEXPRESSResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public PROTOCOLLOEX_CREATO CREAPROTOCOLLOEXPRESSResult;

        public CREAPROTOCOLLOEXPRESSResponseBody()
        {
        }

        public CREAPROTOCOLLOEXPRESSResponseBody(PROTOCOLLOEX_CREATO CREAPROTOCOLLOEXPRESSResult)
        {
            this.CREAPROTOCOLLOEXPRESSResult = CREAPROTOCOLLOEXPRESSResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class ANNULLAPROTOCOLLORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "ANNULLAPROTOCOLLO", Namespace = "http://tempuri.org/", Order = 0)]
        public ANNULLAPROTOCOLLORequestBody Body;

        public ANNULLAPROTOCOLLORequest()
        {
        }

        public ANNULLAPROTOCOLLORequest(ANNULLAPROTOCOLLORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class ANNULLAPROTOCOLLORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string MOTIVAZIONE;

        public ANNULLAPROTOCOLLORequestBody()
        {
        }

        public ANNULLAPROTOCOLLORequestBody(string SESSIONE, string CODICEPROTOCOLLO, string MOTIVAZIONE)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
            this.MOTIVAZIONE = MOTIVAZIONE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class ANNULLAPROTOCOLLOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "ANNULLAPROTOCOLLOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public ANNULLAPROTOCOLLOResponseBody Body;

        public ANNULLAPROTOCOLLOResponse()
        {
        }

        public ANNULLAPROTOCOLLOResponse(ANNULLAPROTOCOLLOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class ANNULLAPROTOCOLLOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string ANNULLAPROTOCOLLOResult;

        public ANNULLAPROTOCOLLOResponseBody()
        {
        }

        public ANNULLAPROTOCOLLOResponseBody(string ANNULLAPROTOCOLLOResult)
        {
            this.ANNULLAPROTOCOLLOResult = ANNULLAPROTOCOLLOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAALLEGATORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAALLEGATO", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAALLEGATORequestBody Body;

        public CREAALLEGATORequest()
        {
        }

        public CREAALLEGATORequest(CREAALLEGATORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAALLEGATORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ANNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string NUMEROPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string FLAGPRINCIPALE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string NOMEFILE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string BASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string PATHFULLFILE;

        public CREAALLEGATORequestBody()
        {
        }

        public CREAALLEGATORequestBody(string SESSIONE, string ANNO, string NUMEROPROTOCOLLO, string DESCRIZIONE, string FLAGPRINCIPALE, string NOMEFILE, string BASE64, string PATHFULLFILE)
        {
            this.SESSIONE = SESSIONE;
            this.ANNO = ANNO;
            this.NUMEROPROTOCOLLO = NUMEROPROTOCOLLO;
            this.DESCRIZIONE = DESCRIZIONE;
            this.FLAGPRINCIPALE = FLAGPRINCIPALE;
            this.NOMEFILE = NOMEFILE;
            this.BASE64 = BASE64;
            this.PATHFULLFILE = PATHFULLFILE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CREAALLEGATOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CREAALLEGATOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CREAALLEGATOResponseBody Body;

        public CREAALLEGATOResponse()
        {
        }

        public CREAALLEGATOResponse(CREAALLEGATOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CREAALLEGATOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CREAALLEGATOResult;

        public CREAALLEGATOResponseBody()
        {
        }

        public CREAALLEGATOResponseBody(string CREAALLEGATOResult)
        {
            this.CREAALLEGATOResult = CREAALLEGATOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RESTITUISCIALLEGATORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RESTITUISCIALLEGATO", Namespace = "http://tempuri.org/", Order = 0)]
        public RESTITUISCIALLEGATORequestBody Body;

        public RESTITUISCIALLEGATORequest()
        {
        }

        public RESTITUISCIALLEGATORequest(RESTITUISCIALLEGATORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RESTITUISCIALLEGATORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ANNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CODICEALLEGATO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string FLAGBASE64;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string PATHFULLFILE;

        public RESTITUISCIALLEGATORequestBody()
        {
        }

        public RESTITUISCIALLEGATORequestBody(string SESSIONE, string ANNO, string CODICEALLEGATO, string FLAGBASE64, string PATHFULLFILE)
        {
            this.SESSIONE = SESSIONE;
            this.ANNO = ANNO;
            this.CODICEALLEGATO = CODICEALLEGATO;
            this.FLAGBASE64 = FLAGBASE64;
            this.PATHFULLFILE = PATHFULLFILE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RESTITUISCIALLEGATOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RESTITUISCIALLEGATOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RESTITUISCIALLEGATOResponseBody Body;

        public RESTITUISCIALLEGATOResponse()
        {
        }

        public RESTITUISCIALLEGATOResponse(RESTITUISCIALLEGATOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RESTITUISCIALLEGATOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INDICE_ALLEGATO RESTITUISCIALLEGATOResult;

        public RESTITUISCIALLEGATOResponseBody()
        {
        }

        public RESTITUISCIALLEGATOResponseBody(INDICE_ALLEGATO RESTITUISCIALLEGATOResult)
        {
            this.RESTITUISCIALLEGATOResult = RESTITUISCIALLEGATOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CANCELLAALLEGATORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CANCELLAALLEGATO", Namespace = "http://tempuri.org/", Order = 0)]
        public CANCELLAALLEGATORequestBody Body;

        public CANCELLAALLEGATORequest()
        {
        }

        public CANCELLAALLEGATORequest(CANCELLAALLEGATORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CANCELLAALLEGATORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ANNO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CODICEALLEGATO;

        public CANCELLAALLEGATORequestBody()
        {
        }

        public CANCELLAALLEGATORequestBody(string SESSIONE, string ANNO, string CODICEALLEGATO)
        {
            this.SESSIONE = SESSIONE;
            this.ANNO = ANNO;
            this.CODICEALLEGATO = CODICEALLEGATO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CANCELLAALLEGATOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CANCELLAALLEGATOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CANCELLAALLEGATOResponseBody Body;

        public CANCELLAALLEGATOResponse()
        {
        }

        public CANCELLAALLEGATOResponse(CANCELLAALLEGATOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CANCELLAALLEGATOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string CANCELLAALLEGATOResult;

        public CANCELLAALLEGATOResponseBody()
        {
        }

        public CANCELLAALLEGATOResponseBody(string CANCELLAALLEGATOResult)
        {
            this.CANCELLAALLEGATOResult = CANCELLAALLEGATOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class PROTOCOLLOASSEGNAZIONERequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "PROTOCOLLOASSEGNAZIONE", Namespace = "http://tempuri.org/", Order = 0)]
        public PROTOCOLLOASSEGNAZIONERequestBody Body;

        public PROTOCOLLOASSEGNAZIONERequest()
        {
        }

        public PROTOCOLLOASSEGNAZIONERequest(PROTOCOLLOASSEGNAZIONERequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLOASSEGNAZIONERequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string TIPOASSEGNAZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string TIPOOPERAZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public DATO_MULTI CODICIUNITAORGANIZZATIVE;

        public PROTOCOLLOASSEGNAZIONERequestBody()
        {
        }

        public PROTOCOLLOASSEGNAZIONERequestBody(string SESSIONE, string CODICEPROTOCOLLO, string TIPOASSEGNAZIONE, string TIPOOPERAZIONE, DATO_MULTI CODICIUNITAORGANIZZATIVE)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
            this.TIPOASSEGNAZIONE = TIPOASSEGNAZIONE;
            this.TIPOOPERAZIONE = TIPOOPERAZIONE;
            this.CODICIUNITAORGANIZZATIVE = CODICIUNITAORGANIZZATIVE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class PROTOCOLLOASSEGNAZIONEResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "PROTOCOLLOASSEGNAZIONEResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public PROTOCOLLOASSEGNAZIONEResponseBody Body;

        public PROTOCOLLOASSEGNAZIONEResponse()
        {
        }

        public PROTOCOLLOASSEGNAZIONEResponse(PROTOCOLLOASSEGNAZIONEResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLOASSEGNAZIONEResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string PROTOCOLLOASSEGNAZIONEResult;

        public PROTOCOLLOASSEGNAZIONEResponseBody()
        {
        }

        public PROTOCOLLOASSEGNAZIONEResponseBody(string PROTOCOLLOASSEGNAZIONEResult)
        {
            this.PROTOCOLLOASSEGNAZIONEResult = PROTOCOLLOASSEGNAZIONEResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class PROTOCOLLOTRASMISSIONERequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "PROTOCOLLOTRASMISSIONE", Namespace = "http://tempuri.org/", Order = 0)]
        public PROTOCOLLOTRASMISSIONERequestBody Body;

        public PROTOCOLLOTRASMISSIONERequest()
        {
        }

        public PROTOCOLLOTRASMISSIONERequest(PROTOCOLLOTRASMISSIONERequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLOTRASMISSIONERequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        public PROTOCOLLOTRASMISSIONERequestBody()
        {
        }

        public PROTOCOLLOTRASMISSIONERequestBody(string SESSIONE, string CODICEPROTOCOLLO)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class PROTOCOLLOTRASMISSIONEResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "PROTOCOLLOTRASMISSIONEResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public PROTOCOLLOTRASMISSIONEResponseBody Body;

        public PROTOCOLLOTRASMISSIONEResponse()
        {
        }

        public PROTOCOLLOTRASMISSIONEResponse(PROTOCOLLOTRASMISSIONEResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class PROTOCOLLOTRASMISSIONEResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public PROTOCOLLOTRASMISSIONEResult PROTOCOLLOTRASMISSIONEResult;

        public PROTOCOLLOTRASMISSIONEResponseBody()
        {
        }

        public PROTOCOLLOTRASMISSIONEResponseBody(PROTOCOLLOTRASMISSIONEResult PROTOCOLLOTRASMISSIONEResult)
        {
            this.PROTOCOLLOTRASMISSIONEResult = PROTOCOLLOTRASMISSIONEResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INVIAPROTOCOLLOVIAPECRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INVIAPROTOCOLLOVIAPEC", Namespace = "http://tempuri.org/", Order = 0)]
        public INVIAPROTOCOLLOVIAPECRequestBody Body;

        public INVIAPROTOCOLLOVIAPECRequest()
        {
        }

        public INVIAPROTOCOLLOVIAPECRequest(INVIAPROTOCOLLOVIAPECRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INVIAPROTOCOLLOVIAPECRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICEPROTOCOLLO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string ACCOUNT;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public DATO_MULTI DESTINATARI;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public DATO_MULTI CODICEALLEGATI;

        public INVIAPROTOCOLLOVIAPECRequestBody()
        {
        }

        public INVIAPROTOCOLLOVIAPECRequestBody(string SESSIONE, string CODICEPROTOCOLLO, string ACCOUNT, DATO_MULTI DESTINATARI, DATO_MULTI CODICEALLEGATI)
        {
            this.SESSIONE = SESSIONE;
            this.CODICEPROTOCOLLO = CODICEPROTOCOLLO;
            this.ACCOUNT = ACCOUNT;
            this.DESTINATARI = DESTINATARI;
            this.CODICEALLEGATI = CODICEALLEGATI;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INVIAPROTOCOLLOVIAPECResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INVIAPROTOCOLLOVIAPECResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public INVIAPROTOCOLLOVIAPECResponseBody Body;

        public INVIAPROTOCOLLOVIAPECResponse()
        {
        }

        public INVIAPROTOCOLLOVIAPECResponse(INVIAPROTOCOLLOVIAPECResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INVIAPROTOCOLLOVIAPECResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INVIAPROTOCOLLOVIAPECResult INVIAPROTOCOLLOVIAPECResult;

        public INVIAPROTOCOLLOVIAPECResponseBody()
        {
        }

        public INVIAPROTOCOLLOVIAPECResponseBody(INVIAPROTOCOLLOVIAPECResult INVIAPROTOCOLLOVIAPECResult)
        {
            this.INVIAPROTOCOLLOVIAPECResult = INVIAPROTOCOLLOVIAPECResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCATITOLARIORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCATITOLARIO", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCATITOLARIORequestBody Body;

        public RICERCATITOLARIORequest()
        {
        }

        public RICERCATITOLARIORequest(RICERCATITOLARIORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCATITOLARIORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string CODICETITOLARIO;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string UNITAORGANIZZATIVA;

        public RICERCATITOLARIORequestBody()
        {
        }

        public RICERCATITOLARIORequestBody(string SESSIONE, string CODICETITOLARIO, string DESCRIZIONE, string UNITAORGANIZZATIVA)
        {
            this.SESSIONE = SESSIONE;
            this.CODICETITOLARIO = CODICETITOLARIO;
            this.DESCRIZIONE = DESCRIZIONE;
            this.UNITAORGANIZZATIVA = UNITAORGANIZZATIVA;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCATITOLARIOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCATITOLARIOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCATITOLARIOResponseBody Body;

        public RICERCATITOLARIOResponse()
        {
        }

        public RICERCATITOLARIOResponse(RICERCATITOLARIOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCATITOLARIOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public MULTI_TITOLARIO RICERCATITOLARIOResult;

        public RICERCATITOLARIOResponseBody()
        {
        }

        public RICERCATITOLARIOResponseBody(MULTI_TITOLARIO RICERCATITOLARIOResult)
        {
            this.RICERCATITOLARIOResult = RICERCATITOLARIOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAUNITAORGANIZZATIVERequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAUNITAORGANIZZATIVE", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAUNITAORGANIZZATIVERequestBody Body;

        public RICERCAUNITAORGANIZZATIVERequest()
        {
        }

        public RICERCAUNITAORGANIZZATIVERequest(RICERCAUNITAORGANIZZATIVERequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAUNITAORGANIZZATIVERequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string DESCRIZIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string TIPO;

        public RICERCAUNITAORGANIZZATIVERequestBody()
        {
        }

        public RICERCAUNITAORGANIZZATIVERequestBody(string SESSIONE, string DESCRIZIONE, string TIPO)
        {
            this.SESSIONE = SESSIONE;
            this.DESCRIZIONE = DESCRIZIONE;
            this.TIPO = TIPO;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RICERCAUNITAORGANIZZATIVEResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "RICERCAUNITAORGANIZZATIVEResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public RICERCAUNITAORGANIZZATIVEResponseBody Body;

        public RICERCAUNITAORGANIZZATIVEResponse()
        {
        }

        public RICERCAUNITAORGANIZZATIVEResponse(RICERCAUNITAORGANIZZATIVEResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class RICERCAUNITAORGANIZZATIVEResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public MULTI_UNITAORGANIZZATIVA RICERCAUNITAORGANIZZATIVEResult;

        public RICERCAUNITAORGANIZZATIVEResponseBody()
        {
        }

        public RICERCAUNITAORGANIZZATIVEResponseBody(MULTI_UNITAORGANIZZATIVA RICERCAUNITAORGANIZZATIVEResult)
        {
            this.RICERCAUNITAORGANIZZATIVEResult = RICERCAUNITAORGANIZZATIVEResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEUNITAORGANIZZATIVARequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEUNITAORGANIZZATIVA", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEUNITAORGANIZZATIVARequestBody Body;

        public INDICEUNITAORGANIZZATIVARequest()
        {
        }

        public INDICEUNITAORGANIZZATIVARequest(INDICEUNITAORGANIZZATIVARequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEUNITAORGANIZZATIVARequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string DESCRIZIONE;

        public INDICEUNITAORGANIZZATIVARequestBody()
        {
        }

        public INDICEUNITAORGANIZZATIVARequestBody(string SESSIONE, string DESCRIZIONE)
        {
            this.SESSIONE = SESSIONE;
            this.DESCRIZIONE = DESCRIZIONE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class INDICEUNITAORGANIZZATIVAResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "INDICEUNITAORGANIZZATIVAResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public INDICEUNITAORGANIZZATIVAResponseBody Body;

        public INDICEUNITAORGANIZZATIVAResponse()
        {
        }

        public INDICEUNITAORGANIZZATIVAResponse(INDICEUNITAORGANIZZATIVAResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class INDICEUNITAORGANIZZATIVAResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public INDICE_UNITAORGANIZZATIVA INDICEUNITAORGANIZZATIVAResult;

        public INDICEUNITAORGANIZZATIVAResponseBody()
        {
        }

        public INDICEUNITAORGANIZZATIVAResponseBody(INDICE_UNITAORGANIZZATIVA INDICEUNITAORGANIZZATIVAResult)
        {
            this.INDICEUNITAORGANIZZATIVAResult = INDICEUNITAORGANIZZATIVAResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class LISTATIPODOCUMENTORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "LISTATIPODOCUMENTO", Namespace = "http://tempuri.org/", Order = 0)]
        public LISTATIPODOCUMENTORequestBody Body;

        public LISTATIPODOCUMENTORequest()
        {
        }

        public LISTATIPODOCUMENTORequest(LISTATIPODOCUMENTORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class LISTATIPODOCUMENTORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        public LISTATIPODOCUMENTORequestBody()
        {
        }

        public LISTATIPODOCUMENTORequestBody(string SESSIONE)
        {
            this.SESSIONE = SESSIONE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class LISTATIPODOCUMENTOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "LISTATIPODOCUMENTOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public LISTATIPODOCUMENTOResponseBody Body;

        public LISTATIPODOCUMENTOResponse()
        {
        }

        public LISTATIPODOCUMENTOResponse(LISTATIPODOCUMENTOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class LISTATIPODOCUMENTOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public RISPOSTA_MULTI_VALORE LISTATIPODOCUMENTOResult;

        public LISTATIPODOCUMENTOResponseBody()
        {
        }

        public LISTATIPODOCUMENTOResponseBody(RISPOSTA_MULTI_VALORE LISTATIPODOCUMENTOResult)
        {
            this.LISTATIPODOCUMENTOResult = LISTATIPODOCUMENTOResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class LISTACATEGORIEFASCICOLIRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "LISTACATEGORIEFASCICOLI", Namespace = "http://tempuri.org/", Order = 0)]
        public LISTACATEGORIEFASCICOLIRequestBody Body;

        public LISTACATEGORIEFASCICOLIRequest()
        {
        }

        public LISTACATEGORIEFASCICOLIRequest(LISTACATEGORIEFASCICOLIRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class LISTACATEGORIEFASCICOLIRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SESSIONE;

        public LISTACATEGORIEFASCICOLIRequestBody()
        {
        }

        public LISTACATEGORIEFASCICOLIRequestBody(string SESSIONE)
        {
            this.SESSIONE = SESSIONE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class LISTACATEGORIEFASCICOLIResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "LISTACATEGORIEFASCICOLIResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public LISTACATEGORIEFASCICOLIResponseBody Body;

        public LISTACATEGORIEFASCICOLIResponse()
        {
        }

        public LISTACATEGORIEFASCICOLIResponse(LISTACATEGORIEFASCICOLIResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class LISTACATEGORIEFASCICOLIResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public RISPOSTA_MULTI_VALORE LISTACATEGORIEFASCICOLIResult;

        public LISTACATEGORIEFASCICOLIResponseBody()
        {
        }

        public LISTACATEGORIEFASCICOLIResponseBody(RISPOSTA_MULTI_VALORE LISTACATEGORIEFASCICOLIResult)
        {
            this.LISTACATEGORIEFASCICOLIResult = LISTACATEGORIEFASCICOLIResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CONVERTIINMULTIVALORERequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CONVERTIINMULTIVALORE", Namespace = "http://tempuri.org/", Order = 0)]
        public CONVERTIINMULTIVALORERequestBody Body;

        public CONVERTIINMULTIVALORERequest()
        {
        }

        public CONVERTIINMULTIVALORERequest(CONVERTIINMULTIVALORERequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CONVERTIINMULTIVALORERequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string VALORESINGLE;

        public CONVERTIINMULTIVALORERequestBody()
        {
        }

        public CONVERTIINMULTIVALORERequestBody(string VALORESINGLE)
        {
            this.VALORESINGLE = VALORESINGLE;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CONVERTIINMULTIVALOREResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CONVERTIINMULTIVALOREResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public CONVERTIINMULTIVALOREResponseBody Body;

        public CONVERTIINMULTIVALOREResponse()
        {
        }

        public CONVERTIINMULTIVALOREResponse(CONVERTIINMULTIVALOREResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CONVERTIINMULTIVALOREResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public DATO_MULTI CONVERTIINMULTIVALOREResult;

        public CONVERTIINMULTIVALOREResponseBody()
        {
        }

        public CONVERTIINMULTIVALOREResponseBody(DATO_MULTI CONVERTIINMULTIVALOREResult)
        {
            this.CONVERTIINMULTIVALOREResult = CONVERTIINMULTIVALOREResult;
        }
    }


 
}
