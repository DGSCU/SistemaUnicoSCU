#pragma checksum "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9092af7a6c4a90be18da57f7a26a69e3bf0f2a30"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Accesso_Spid), @"mvc.1.0.view", @"/Views/Accesso/Spid.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\temp\SistemaUnico\Registrazione\Views\_ViewImports.cshtml"
using RegistrazioneSistemaUnico;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\temp\SistemaUnico\Registrazione\Views\_ViewImports.cshtml"
using RegistrazioneSistemaUnico.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
using static RegistrazioneSistemaUnico.Helpers.Helper;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9092af7a6c4a90be18da57f7a26a69e3bf0f2a30", @"/Views/Accesso/Spid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"08f4fa5a7f4165ac15f3eaad47054f25f75946a0", @"/Views/_ViewImports.cshtml")]
    public class Views_Accesso_Spid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Accesso>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
  
	ViewData["Title"] = "Accesso SSPID";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"text-center\">\r\n\t<h1 class=\"display-4\">Accesso con SPID</h1>\r\n\t<p>Simulazione accesso tramite SPID</p>\r\n");
#nullable restore
#line 9 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
     using (Html.BeginForm("LoginSpid", "Accesso", FormMethod.Post))
	{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<div class=\"row\">\r\n\t\t");
#nullable restore
#line 12 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
   Write(Html.InputFor(x => x.CodiceFiscale, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\t\t");
#nullable restore
#line 13 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
   Write(Html.InputFor(x => x.DataNascita, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\t</div>\r\n\t<div class=\"row\">\r\n\t\t");
#nullable restore
#line 16 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
   Write(Html.InputFor(x => x.Nome, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\t\t");
#nullable restore
#line 17 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
   Write(Html.InputFor(x => x.Cognome, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\t</div>\r\n\t<input class=\"btn btn-primary col-md-3 col-sm-6\" type=\"submit\" value=\"Accesso\">\r\n");
#nullable restore
#line 20 "C:\temp\SistemaUnico\Registrazione\Views\Accesso\Spid.cshtml"
	}

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Accesso> Html { get; private set; }
    }
}
#pragma warning restore 1591