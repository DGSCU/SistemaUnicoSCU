using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Servizi
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "SpidService.IService")]
    public interface IService
    {


        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IService/getUserInfo", ReplyAction = "http://tempuri.org/IService/getUserInfoResponse")]
        userInfo getUserInfo(string t);

    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "userInfo", Namespace = "http://schemas.datacontract.org/2004/07/")]
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
    [System.Runtime.Serialization.DataContractAttribute(Name = "AuthRequest", Namespace = "http://schemas.datacontract.org/2004/07/")]
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
    [System.Runtime.Serialization.DataContractAttribute(Name = "RespProvider", Namespace = "http://schemas.datacontract.org/2004/07/")]
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
    [System.Runtime.Serialization.DataContractAttribute(Name = "ServiceProvider", Namespace = "http://schemas.datacontract.org/2004/07/")]
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
}
