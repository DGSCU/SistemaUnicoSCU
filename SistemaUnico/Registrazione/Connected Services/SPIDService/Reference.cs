//------------------------------------------------------------------------------
// <auto-generated>
//     Questo codice è stato generato da uno strumento.
//
//     Le modifiche apportate a questo file possono causare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpidService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="userInfo", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class userInfo : object
    {
        
        private string AddressField;
        
        private string CountyOfBirthField;
        
        private string DateOfBirthField;
        
        private string EmailField;
        
        private string ExpirationDateField;
        
        private string FiscalNumberField;
        
        private string GenderField;
        
        private string IdCardField;
        
        private string MobilePhoneField;
        
        private string NameField;
        
        private string PlaceOfBirthField;
        
        private string SpidcodeField;
        
        private string SurnameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Address
        {
            get
            {
                return this.AddressField;
            }
            set
            {
                this.AddressField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CountyOfBirth
        {
            get
            {
                return this.CountyOfBirthField;
            }
            set
            {
                this.CountyOfBirthField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DateOfBirth
        {
            get
            {
                return this.DateOfBirthField;
            }
            set
            {
                this.DateOfBirthField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email
        {
            get
            {
                return this.EmailField;
            }
            set
            {
                this.EmailField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExpirationDate
        {
            get
            {
                return this.ExpirationDateField;
            }
            set
            {
                this.ExpirationDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FiscalNumber
        {
            get
            {
                return this.FiscalNumberField;
            }
            set
            {
                this.FiscalNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gender
        {
            get
            {
                return this.GenderField;
            }
            set
            {
                this.GenderField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdCard
        {
            get
            {
                return this.IdCardField;
            }
            set
            {
                this.IdCardField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MobilePhone
        {
            get
            {
                return this.MobilePhoneField;
            }
            set
            {
                this.MobilePhoneField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PlaceOfBirth
        {
            get
            {
                return this.PlaceOfBirthField;
            }
            set
            {
                this.PlaceOfBirthField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Spidcode
        {
            get
            {
                return this.SpidcodeField;
            }
            set
            {
                this.SpidcodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Surname
        {
            get
            {
                return this.SurnameField;
            }
            set
            {
                this.SurnameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AuthRequest", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class AuthRequest : object
    {
        
        private string AuthReqField;
        
        private string AuthnReqIDField;
        
        private string AuthnReqIssueIstantField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AuthReq
        {
            get
            {
                return this.AuthReqField;
            }
            set
            {
                this.AuthReqField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AuthnReqID
        {
            get
            {
                return this.AuthnReqIDField;
            }
            set
            {
                this.AuthnReqIDField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AuthnReqIssueIstant
        {
            get
            {
                return this.AuthnReqIssueIstantField;
            }
            set
            {
                this.AuthnReqIssueIstantField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespProvider", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class RespProvider : object
    {
        
        private string Assertion_IDField;
        
        private string Assertion_subjectField;
        
        private string Assertion_subject_NameQualifierField;
        
        private string AuthnReq_IDField;
        
        private string Resp_IDField;
        
        private string Resp_IssueInstanField;
        
        private string Resp_IssuerField;
        
        private string ResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Assertion_ID
        {
            get
            {
                return this.Assertion_IDField;
            }
            set
            {
                this.Assertion_IDField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Assertion_subject
        {
            get
            {
                return this.Assertion_subjectField;
            }
            set
            {
                this.Assertion_subjectField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Assertion_subject_NameQualifier
        {
            get
            {
                return this.Assertion_subject_NameQualifierField;
            }
            set
            {
                this.Assertion_subject_NameQualifierField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AuthnReq_ID
        {
            get
            {
                return this.AuthnReq_IDField;
            }
            set
            {
                this.AuthnReq_IDField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Resp_ID
        {
            get
            {
                return this.Resp_IDField;
            }
            set
            {
                this.Resp_IDField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Resp_IssueInstan
        {
            get
            {
                return this.Resp_IssueInstanField;
            }
            set
            {
                this.Resp_IssueInstanField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Resp_Issuer
        {
            get
            {
                return this.Resp_IssuerField;
            }
            set
            {
                this.Resp_IssuerField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Response
        {
            get
            {
                return this.ResponseField;
            }
            set
            {
                this.ResponseField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceProvider", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class ServiceProvider : object
    {
        
        private string ServizioField;
        
        private int attributeIndexField;
        
        private string idServizioField;
        
        private string pathField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Servizio
        {
            get
            {
                return this.ServizioField;
            }
            set
            {
                this.ServizioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int attributeIndex
        {
            get
            {
                return this.attributeIndexField;
            }
            set
            {
                this.attributeIndexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string idServizio
        {
            get
            {
                return this.idServizioField;
            }
            set
            {
                this.idServizioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SpidService.IService")]
    public interface IService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/saveUserInfo", ReplyAction="http://tempuri.org/IService/saveUserInfoResponse")]
        string saveUserInfo(SpidService.userInfo u);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/saveUserInfo", ReplyAction="http://tempuri.org/IService/saveUserInfoResponse")]
        System.Threading.Tasks.Task<string> saveUserInfoAsync(SpidService.userInfo u);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/getUserInfo", ReplyAction="http://tempuri.org/IService/getUserInfoResponse")]
        SpidService.userInfo getUserInfo(string t);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/getUserInfo", ReplyAction="http://tempuri.org/IService/getUserInfoResponse")]
        System.Threading.Tasks.Task<SpidService.userInfo> getUserInfoAsync(string t);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/getUserCF", ReplyAction="http://tempuri.org/IService/getUserCFResponse")]
        string getUserCF(string t);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/getUserCF", ReplyAction="http://tempuri.org/IService/getUserCFResponse")]
        System.Threading.Tasks.Task<string> getUserCFAsync(string t);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/saveAuthReq", ReplyAction="http://tempuri.org/IService/saveAuthReqResponse")]
        int saveAuthReq(SpidService.AuthRequest a);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/saveAuthReq", ReplyAction="http://tempuri.org/IService/saveAuthReqResponse")]
        System.Threading.Tasks.Task<int> saveAuthReqAsync(SpidService.AuthRequest a);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/saveResp", ReplyAction="http://tempuri.org/IService/saveRespResponse")]
        int saveResp(SpidService.RespProvider r);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/saveResp", ReplyAction="http://tempuri.org/IService/saveRespResponse")]
        System.Threading.Tasks.Task<int> saveRespAsync(SpidService.RespProvider r);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/getService", ReplyAction="http://tempuri.org/IService/getServiceResponse")]
        SpidService.ServiceProvider getService(int s);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/getService", ReplyAction="http://tempuri.org/IService/getServiceResponse")]
        System.Threading.Tasks.Task<SpidService.ServiceProvider> getServiceAsync(int s);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface IServiceChannel : SpidService.IService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<SpidService.IService>, SpidService.IService
    {
        
        /// <summary>
        /// Implementare questo metodo parziale per configurare l'endpoint servizio.
        /// </summary>
        /// <param name="serviceEndpoint">Endpoint da configurare</param>
        /// <param name="clientCredentials">Credenziali del client</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ServiceClient() : 
                base(ServiceClient.GetDefaultBinding(), ServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(ServiceClient.GetBindingForEndpoint(endpointConfiguration), ServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string saveUserInfo(SpidService.userInfo u)
        {
            return base.Channel.saveUserInfo(u);
        }
        
        public System.Threading.Tasks.Task<string> saveUserInfoAsync(SpidService.userInfo u)
        {
            return base.Channel.saveUserInfoAsync(u);
        }
        
        public SpidService.userInfo getUserInfo(string t)
        {
            return base.Channel.getUserInfo(t);
        }
        
        public System.Threading.Tasks.Task<SpidService.userInfo> getUserInfoAsync(string t)
        {
            return base.Channel.getUserInfoAsync(t);
        }
        
        public string getUserCF(string t)
        {
            return base.Channel.getUserCF(t);
        }
        
        public System.Threading.Tasks.Task<string> getUserCFAsync(string t)
        {
            return base.Channel.getUserCFAsync(t);
        }
        
        public int saveAuthReq(SpidService.AuthRequest a)
        {
            return base.Channel.saveAuthReq(a);
        }
        
        public System.Threading.Tasks.Task<int> saveAuthReqAsync(SpidService.AuthRequest a)
        {
            return base.Channel.saveAuthReqAsync(a);
        }
        
        public int saveResp(SpidService.RespProvider r)
        {
            return base.Channel.saveResp(r);
        }
        
        public System.Threading.Tasks.Task<int> saveRespAsync(SpidService.RespProvider r)
        {
            return base.Channel.saveRespAsync(r);
        }
        
        public SpidService.ServiceProvider getService(int s)
        {
            return base.Channel.getService(s);
        }
        
        public System.Threading.Tasks.Task<SpidService.ServiceProvider> getServiceAsync(int s)
        {
            return base.Channel.getServiceAsync(s);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("L\'endpoint denominato \'{0}\' non è stato trovato.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IService))
            {
                return new System.ServiceModel.EndpointAddress("http://intranet/wsSpid/Service.svc");
            }
            throw new System.InvalidOperationException(string.Format("L\'endpoint denominato \'{0}\' non è stato trovato.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return ServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return ServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IService,
        }
    }
}
