#pragma checksum "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5837e4b705d495cb68b65bfe6bc5dff68ab633d1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Registrazione_VariazioneRappresentante), @"mvc.1.0.view", @"/Views/Registrazione/VariazioneRappresentante.cshtml")]
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
#line 1 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
using static RegistrazioneSistemaUnico.Helpers.Helper;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
using RegistrazioneSistemaUnico.Models.Forms;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5837e4b705d495cb68b65bfe6bc5dff68ab633d1", @"/Views/Registrazione/VariazioneRappresentante.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"08f4fa5a7f4165ac15f3eaad47054f25f75946a0", @"/Views/_ViewImports.cshtml")]
    public class Views_Registrazione_VariazioneRappresentante : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<VariazioneRappresentanteForm>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
  

	ViewData["Title"] = "Home Page";
	Dictionary<string, string> categorie = (Dictionary<string, string>)ViewData["IdCategoriaEnte"];
	

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
Write(Html.Sommario("Si sono verificati i seguenti errori:"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
#nullable restore
#line 12 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
 using (Html.BeginForm())
{


#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t<h2>Variazione Rappresentante Legale Ente Titolare del Servizio Civile Nazionale</h2>\r\n\t\t<br />\r\n");
#nullable restore
#line 17 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
   Write(Html.InputFor(x => x.CodiceFiscaleEnte));

#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t<a");
            BeginWriteAttribute("href", " href=\"", 525, "\"", 562, 1);
#nullable restore
#line 18 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"
WriteAttributeValue("", 532, Url.Action("Index","Accesso"), 532, 30, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-link btn-primary col-md-3 col-sm-6\">Indietro</a>\r\n");
            WriteLiteral("\t\t<button class=\"btn btn-primary col-md-3 col-sm-6\" type=\"submit\" >Registra</button>\r\n");
#nullable restore
#line 21 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\VariazioneRappresentante.cshtml"

}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<VariazioneRappresentanteForm> Html { get; private set; }
    }
}
#pragma warning restore 1591