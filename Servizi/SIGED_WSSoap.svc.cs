using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SIGED_WSSoap
{

	public class SIGED_WSSoap : ISIGED_WSSoap
	{
		public ANNULLAPROTOCOLLOResponse ANNULLAPROTOCOLLO(ANNULLAPROTOCOLLORequest request)
		{
			throw new NotImplementedException();
		}

		public CANCELLAALLEGATOResponse CANCELLAALLEGATO(CANCELLAALLEGATORequest request)
		{
			throw new NotImplementedException();
		}

		public CONVERTIINMULTIVALOREResponse CONVERTIINMULTIVALORE(CONVERTIINMULTIVALORERequest request)
		{
			throw new NotImplementedException();
		}

		public CREAALLEGATOResponse CREAALLEGATO(CREAALLEGATORequest request)
		{
			throw new NotImplementedException();
		}

		public CREAANAGRAFICAResponse CREAANAGRAFICA(CREAANAGRAFICARequest request)
		{
			throw new NotImplementedException();
		}

		public CREACOLLEGAMENTOFASCICOLOResponse CREACOLLEGAMENTOFASCICOLO(CREACOLLEGAMENTOFASCICOLORequest request)
		{
			throw new NotImplementedException();
		}

		public CREADOCUMENTOINTERNOResponse CREADOCUMENTOINTERNO(CREADOCUMENTOINTERNORequest request)
		{
			throw new NotImplementedException();
		}

		public CREAFASCICOLOResponse CREAFASCICOLO(CREAFASCICOLORequest request)
		{
			throw new NotImplementedException();
		}

		public CREAFASCICOLOEXPRESSResponse CREAFASCICOLOEXPRESS(CREAFASCICOLOEXPRESSRequest request)
		{
			return new CREAFASCICOLOEXPRESSResponse()
			{
				Body = new CREAFASCICOLOEXPRESSResponseBody()
				{
					CREAFASCICOLOEXPRESSResult = new FASCICOLOEX_CREATO()
					{
						ESITO = "00000 - OK"
					}
				}
			};
		}

		public CREAFASCICOLOMULTIPLOResponse CREAFASCICOLOMULTIPLO(CREAFASCICOLOMULTIPLORequest request)
		{
			throw new NotImplementedException();
		}

		public CREAPROTOCOLLOResponse CREAPROTOCOLLO(CREAPROTOCOLLORequest request)
		{
			throw new NotImplementedException();
		}

		public CREAPROTOCOLLOEXPRESSResponse CREAPROTOCOLLOEXPRESS(CREAPROTOCOLLOEXPRESSRequest request)
		{
			return new CREAPROTOCOLLOEXPRESSResponse()
			{
				Body = new CREAPROTOCOLLOEXPRESSResponseBody()
				{
					CREAPROTOCOLLOEXPRESSResult = new PROTOCOLLOEX_CREATO()
					{
						ESITO = "00000 - OK",
						NUMEROPROTOCOLLO = "00001",
						DATAPROTOCOLLO = DateTime.Now.ToString("dd/MM/yyyy")
					}
				}
			};
		}

		public CREASOTTOFASCICOLOResponse CREASOTTOFASCICOLO(CREASOTTOFASCICOLORequest request)
		{
			throw new NotImplementedException();
		}

		public CREASOTTOFASCICOLOEXPRESSResponse CREASOTTOFASCICOLOEXPRESS(CREASOTTOFASCICOLOEXPRESSRequest request)
		{
			throw new NotImplementedException();
		}

		public INDICEANAGRAFICAResponse INDICEANAGRAFICA(INDICEANAGRAFICARequest request)
		{
			throw new NotImplementedException();
		}

		public INDICEFASCICOLOResponse INDICEFASCICOLO(INDICEFASCICOLORequest request)
		{
			throw new NotImplementedException();
		}

		public INDICEPROTOCOLLOResponse INDICEPROTOCOLLO(INDICEPROTOCOLLORequest request)
		{
			throw new NotImplementedException();
		}

		public INDICEUNITAORGANIZZATIVAResponse INDICEUNITAORGANIZZATIVA(INDICEUNITAORGANIZZATIVARequest request)
		{
			throw new NotImplementedException();
		}

		public INVIAPROTOCOLLOVIAPECResponse INVIAPROTOCOLLOVIAPEC(INVIAPROTOCOLLOVIAPECRequest request)
		{
			throw new NotImplementedException();
		}

		public LISTACATEGORIEFASCICOLIResponse LISTACATEGORIEFASCICOLI(LISTACATEGORIEFASCICOLIRequest request)
		{
			throw new NotImplementedException();
		}

		public LISTATIPODOCUMENTOResponse LISTATIPODOCUMENTO(LISTATIPODOCUMENTORequest request)
		{
			throw new NotImplementedException();
		}

		public PROTOCOLLOASSEGNAZIONEResponse PROTOCOLLOASSEGNAZIONE(PROTOCOLLOASSEGNAZIONERequest request)
		{
			throw new NotImplementedException();
		}

		public PROTOCOLLOTRASMISSIONEResponse PROTOCOLLOTRASMISSIONE(PROTOCOLLOTRASMISSIONERequest request)
		{
			throw new NotImplementedException();
		}

		public RESTITUISCIALLEGATOResponse RESTITUISCIALLEGATO(RESTITUISCIALLEGATORequest request)
		{
			throw new NotImplementedException();
		}

		public RESTITUISCIDOCUMENTOINTERNOResponse RESTITUISCIDOCUMENTOINTERNO(RESTITUISCIDOCUMENTOINTERNORequest request)
		{
			throw new NotImplementedException();
		}

		public RESTITUISCILINKFASCICOLOResponse RESTITUISCILINKFASCICOLO(RESTITUISCILINKFASCICOLORequest request)
		{
			throw new NotImplementedException();
		}

		public RICERCAANAGRAFICHEResponse RICERCAANAGRAFICHE(RICERCAANAGRAFICHERequest request)
		{
			throw new NotImplementedException();
		}

		public RICERCAFASCICOLIResponse RICERCAFASCICOLI(RICERCAFASCICOLIRequest request)
		{
			throw new NotImplementedException();
		}

		public RICERCAPROTOCOLLIResponse RICERCAPROTOCOLLI(RICERCAPROTOCOLLIRequest request)
		{
			throw new NotImplementedException();
		}

		public RICERCATITOLARIOResponse RICERCATITOLARIO(RICERCATITOLARIORequest request)
		{
			throw new NotImplementedException();
		}

		public RICERCAUNITAORGANIZZATIVEResponse RICERCAUNITAORGANIZZATIVE(RICERCAUNITAORGANIZZATIVERequest request)
		{
			throw new NotImplementedException();
		}
	}
}
