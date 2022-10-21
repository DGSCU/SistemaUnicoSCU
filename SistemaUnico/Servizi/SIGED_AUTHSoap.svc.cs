using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SIGED_AUTHSoap
{
	public class SIGED_AUTHSoap : ISIGED_AUTHSoap
	{
		public SWS_CLOSESESSIONResponse SWS_CLOSESESSION(SWS_CLOSESESSIONRequest request)
		{
			throw new NotImplementedException();
		}

		public string SWS_NEWSESSION(string FIRSTNAME,string LASTNAME, string PASSWORD)
		{
			return "00000 - Ok";
		}

		public SWS_NEWSESSIONResponse SWS_NEWSESSION(SWS_NEWSESSIONRequest request)
		{
			return new SWS_NEWSESSIONResponse()
			{
				Body=new SWS_NEWSESSIONResponseBody(){
					SWS_NEWSESSIONResult= "00000 - Ok"
				}
			};
		}

		public SWS_RENEWSESSIONResponse SWS_RENEWSESSION(SWS_RENEWSESSIONRequest request)
		{
			throw new NotImplementedException();
		}

		public SWS_STATUSSESSIONResponse SWS_STATUSSESSION(SWS_STATUSSESSIONRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
