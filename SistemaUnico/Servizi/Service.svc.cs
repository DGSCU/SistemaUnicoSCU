using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Servizi
{
	public class Service : IService
	{
		public ServiceProvider getService(int s)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceProvider> getServiceAsync(int s)
		{
			throw new NotImplementedException();
		}

		public string getUserCF(string t)
		{
			throw new NotImplementedException();
		}

		public Task<string> getUserCFAsync(string t)
		{
			throw new NotImplementedException();
		}

		public userInfo getUserInfo(string t)
		{
			userInfo userInfo  =new userInfo()
			{
				Name = "Simone",
				Surname= "Curti",
				FiscalNumber=t
			};
			return userInfo;
		}

		public Task<userInfo> getUserInfoAsync(string t)
		{
			throw new NotImplementedException();
		}

		public int saveAuthReq(AuthRequest a)
		{
			throw new NotImplementedException();
		}

		public Task<int> saveAuthReqAsync(AuthRequest a)
		{
			throw new NotImplementedException();
		}

		public int saveResp(RespProvider r)
		{
			throw new NotImplementedException();
		}

		public Task<int> saveRespAsync(RespProvider r)
		{
			throw new NotImplementedException();
		}

		public string saveUserInfo(userInfo u)
		{
			throw new NotImplementedException();
		}

		public Task<string> saveUserInfoAsync(userInfo u)
		{
			throw new NotImplementedException();
		}
	}
}
