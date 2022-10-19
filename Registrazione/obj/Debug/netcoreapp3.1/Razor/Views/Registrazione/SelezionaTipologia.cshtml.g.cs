#pragma checksum "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "090ee7b95dccdeb05868dcfc579fcbf66cba4181"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Registrazione_SelezionaTipologia), @"mvc.1.0.view", @"/Views/Registrazione/SelezionaTipologia.cshtml")]
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
#line 1 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
using static RegistrazioneSistemaUnico.Helpers.Helper;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
using RegistrazioneSistemaUnico.Models.Forms;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"090ee7b95dccdeb05868dcfc579fcbf66cba4181", @"/Views/Registrazione/SelezionaTipologia.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"08f4fa5a7f4165ac15f3eaad47054f25f75946a0", @"/Views/_ViewImports.cshtml")]
    public class Views_Registrazione_SelezionaTipologia : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SelezionaEnteForm>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
  

	ViewData["Title"] = "Home Page";
	Dictionary<string, string> categorie = (Dictionary<string, string>)ViewData["IdCategoriaEnte"];
	

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
Write(Html.Sommario("Si sono verificati i seguenti errori:"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
#nullable restore
#line 12 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
 using (Html.BeginForm())
{

	

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
     if (Model.EnteTitolare == true)
	{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t<h2>Registrazione Rappresentante Legale nuovo Ente Titolare</h2>\r\n\t\t<br />\r\n");
#nullable restore
#line 19 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
   Write(Html.InputFor(x => x.CodiceFiscaleEnte));

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
                                                

		foreach (var categoria in categorie)
		{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t\t<button class=\"btn btn-primary col-md-3 col-sm-6\" type=\"submit\" name=\"IdCategoriaEnte\"");
            BeginWriteAttribute("value", " value=\"", 664, "\"", 686, 1);
#nullable restore
#line 23 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
WriteAttributeValue("", 672, categoria.Key, 672, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Ente ");
#nullable restore
#line 23 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
                                                                                                                          Write(categoria.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n\t\t\t<br />\r\n");
#nullable restore
#line 25 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
		}
	}
	else
	{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t<h2>Registrazione Rappresentante Legale Ente di Accoglienza</h2>\r\n\t\t<br />\r\n");
#nullable restore
#line 31 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
   Write(Html.HiddenFor(x => x.CodiceFiscaleEnte));

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
                                                 

		foreach (var categoria in categorie)
		{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t\t<button class=\"btn btn-primary col-md-3 col-sm-6\" type=\"submit\" name=\"IdCategoriaEnte\"");
            BeginWriteAttribute("value", " value=\"", 1010, "\"", 1032, 1);
#nullable restore
#line 35 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
WriteAttributeValue("", 1018, categoria.Key, 1018, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Ente ");
#nullable restore
#line 35 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
                                                                                                                          Write(categoria.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n\t\t\t<br />\r\n");
#nullable restore
#line 37 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
		}

	}

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
Write(Html.HiddenFor(x => x.EnteTitolare));

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\SelezionaTipologia.cshtml"
                                        ;
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SelezionaEnteForm> Html { get; private set; }
    }
}
#pragma warning restore 1591
