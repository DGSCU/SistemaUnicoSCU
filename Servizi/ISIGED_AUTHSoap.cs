using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SIGED_AUTHSoap
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "SIGED_AUTHSoap")]
    public interface ISIGED_AUTHSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/SWS_NEWSESSION", ReplyAction = "*")]
        SWS_NEWSESSIONResponse SWS_NEWSESSION(SWS_NEWSESSIONRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/SWS_CLOSESESSION", ReplyAction = "*")]
        SWS_CLOSESESSIONResponse SWS_CLOSESESSION(SWS_CLOSESESSIONRequest request);


        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/SWS_RENEWSESSION", ReplyAction = "*")]
        SWS_RENEWSESSIONResponse SWS_RENEWSESSION(SWS_RENEWSESSIONRequest request);

        // CODEGEN: Generazione del contratto di messaggio perché il nome di elemento IDSESSION dello spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/SWS_STATUSSESSION", ReplyAction = "*")]
        SWS_STATUSSESSIONResponse SWS_STATUSSESSION(SWS_STATUSSESSIONRequest request);

    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_NEWSESSIONRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_NEWSESSION", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_NEWSESSIONRequestBody Body;

        public SWS_NEWSESSIONRequest()
        {
        }

        public SWS_NEWSESSIONRequest(SWS_NEWSESSIONRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_NEWSESSIONRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string FIRSTNAME;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string LASTNAME;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string PASSWORD;

        public SWS_NEWSESSIONRequestBody()
        {
        }

        public SWS_NEWSESSIONRequestBody(string FIRSTNAME, string LASTNAME, string PASSWORD)
        {
            this.FIRSTNAME = FIRSTNAME;
            this.LASTNAME = LASTNAME;
            this.PASSWORD = PASSWORD;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_NEWSESSIONResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_NEWSESSIONResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_NEWSESSIONResponseBody Body;

        public SWS_NEWSESSIONResponse()
        {
        }

        public SWS_NEWSESSIONResponse(SWS_NEWSESSIONResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_NEWSESSIONResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SWS_NEWSESSIONResult;

        public SWS_NEWSESSIONResponseBody()
        {
        }

        public SWS_NEWSESSIONResponseBody(string SWS_NEWSESSIONResult)
        {
            this.SWS_NEWSESSIONResult = SWS_NEWSESSIONResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_CLOSESESSIONRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_CLOSESESSION", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_CLOSESESSIONRequestBody Body;

        public SWS_CLOSESESSIONRequest()
        {
        }

        public SWS_CLOSESESSIONRequest(SWS_CLOSESESSIONRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_CLOSESESSIONRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string IDSESSION;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string FIRSTNAME;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string LASTNAME;

        public SWS_CLOSESESSIONRequestBody()
        {
        }

        public SWS_CLOSESESSIONRequestBody(string IDSESSION, string FIRSTNAME, string LASTNAME)
        {
            this.IDSESSION = IDSESSION;
            this.FIRSTNAME = FIRSTNAME;
            this.LASTNAME = LASTNAME;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_CLOSESESSIONResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_CLOSESESSIONResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_CLOSESESSIONResponseBody Body;

        public SWS_CLOSESESSIONResponse()
        {
        }

        public SWS_CLOSESESSIONResponse(SWS_CLOSESESSIONResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_CLOSESESSIONResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SWS_CLOSESESSIONResult;

        public SWS_CLOSESESSIONResponseBody()
        {
        }

        public SWS_CLOSESESSIONResponseBody(string SWS_CLOSESESSIONResult)
        {
            this.SWS_CLOSESESSIONResult = SWS_CLOSESESSIONResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_RENEWSESSIONRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_RENEWSESSION", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_RENEWSESSIONRequestBody Body;

        public SWS_RENEWSESSIONRequest()
        {
        }

        public SWS_RENEWSESSIONRequest(SWS_RENEWSESSIONRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_RENEWSESSIONRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string IDSESSION;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string FIRSTNAME;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string LASTNAME;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string PASSWOR;

        public SWS_RENEWSESSIONRequestBody()
        {
        }

        public SWS_RENEWSESSIONRequestBody(string IDSESSION, string FIRSTNAME, string LASTNAME, string PASSWOR)
        {
            this.IDSESSION = IDSESSION;
            this.FIRSTNAME = FIRSTNAME;
            this.LASTNAME = LASTNAME;
            this.PASSWOR = PASSWOR;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_RENEWSESSIONResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_RENEWSESSIONResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_RENEWSESSIONResponseBody Body;

        public SWS_RENEWSESSIONResponse()
        {
        }

        public SWS_RENEWSESSIONResponse(SWS_RENEWSESSIONResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_RENEWSESSIONResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SWS_RENEWSESSIONResult;

        public SWS_RENEWSESSIONResponseBody()
        {
        }

        public SWS_RENEWSESSIONResponseBody(string SWS_RENEWSESSIONResult)
        {
            this.SWS_RENEWSESSIONResult = SWS_RENEWSESSIONResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_STATUSSESSIONRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_STATUSSESSION", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_STATUSSESSIONRequestBody Body;

        public SWS_STATUSSESSIONRequest()
        {
        }

        public SWS_STATUSSESSIONRequest(SWS_STATUSSESSIONRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_STATUSSESSIONRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string IDSESSION;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string FIRSTNAME;

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string LASTNAME;

        public SWS_STATUSSESSIONRequestBody()
        {
        }

        public SWS_STATUSSESSIONRequestBody(string IDSESSION, string FIRSTNAME, string LASTNAME)
        {
            this.IDSESSION = IDSESSION;
            this.FIRSTNAME = FIRSTNAME;
            this.LASTNAME = LASTNAME;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class SWS_STATUSSESSIONResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "SWS_STATUSSESSIONResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public SWS_STATUSSESSIONResponseBody Body;

        public SWS_STATUSSESSIONResponse()
        {
        }

        public SWS_STATUSSESSIONResponse(SWS_STATUSSESSIONResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class SWS_STATUSSESSIONResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public string SWS_STATUSSESSIONResult;

        public SWS_STATUSSESSIONResponseBody()
        {
        }

        public SWS_STATUSSESSIONResponseBody(string SWS_STATUSSESSIONResult)
        {
            this.SWS_STATUSSESSIONResult = SWS_STATUSSESSIONResult;
        }
    }


}
