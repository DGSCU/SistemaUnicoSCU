#pragma checksum "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d184b1c0fc4fd39cc0693a6d641b35552b7c047c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Registrazione_Index), @"mvc.1.0.view", @"/Views/Registrazione/Index.cshtml")]
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
#line 1 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
using static RegistrazioneSistemaUnico.Helpers.Helper;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d184b1c0fc4fd39cc0693a6d641b35552b7c047c", @"/Views/Registrazione/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"08f4fa5a7f4165ac15f3eaad47054f25f75946a0", @"/Views/_ViewImports.cshtml")]
    public class Views_Registrazione_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Registrazione>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
  

	ViewData["Title"] = "Home Page";
	

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
	string warning = TempData["warning"]?.ToString();
	bool variazione = Model.VariazioneRappresentanteLegale == true;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 10 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
 if (variazione)
{
	

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<div class=\"alert alert-warning\" role=\"warning\">\r\n\t\tAttenzione, per questo Ente risulta un Rappresentante Legale diverso. Si sta procedendo alla variazione del Rappresentente Legale.\r\n\t</div>\r\n");
#nullable restore
#line 16 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
Write(Html.Sommario("Si sono verificati i seguenti errori:"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<h3>Inserimento dei dati del nuovo Rappresentante Legale</h3>\r\n\t<br />\r\n\t");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d184b1c0fc4fd39cc0693a6d641b35552b7c047c5290", async() => {
                WriteLiteral("\r\n\r\n\t\t");
#nullable restore
#line 22 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.Albo));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t");
#nullable restore
#line 23 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.VariazioneRappresentanteLegale));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t");
#nullable restore
#line 24 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.Email));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t");
#nullable restore
#line 25 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.EnteTitolare));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 29 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.CodiceFiscaleEnte, "col-md-6", readOnly: true, showRequiredOnLabel: false));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 30 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Denominazione, "col-md-6", readOnly: true, showRequiredOnLabel: false));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 33 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.CodiceFiscaleRappresentanteLegale, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 34 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.DataNominaRappresentanteLegale, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 37 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.DocumentoNomina, cssClass: "col-md-12"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\r\n\t\t<a");
                BeginWriteAttribute("href", " href=\"", 1415, "\"", 1452, 1);
#nullable restore
#line 40 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
WriteAttributeValue("", 1422, Url.Action("Index","Accesso"), 1422, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" class=\"btn btn-link btn-primary col-md-3 col-sm-6\">Indietro</a>\r\n\t\t<button type=\"submit\" class=\"btn btn-primary col-md-3 col-sm-6\">Avanti</button>\r\n\t");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "action", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 20 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
AddHtmlAttributeValue("", 678, Url.Action("Riepilogo"), 678, 24, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 43 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
}
else
{
	

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<h3>Inserimento dei dati dell\'Ente ");
#nullable restore
#line 48 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
                                  Write(Model.TipoEnte);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n\t<br />\r\n");
#nullable restore
#line 50 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
     if (!string.IsNullOrEmpty(warning))
	{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t\t<div class=\"alert alert-warning\" role=\"warning\">\r\n\t\t\t");
#nullable restore
#line 53 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(warning);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\t\t</div>\r\n");
#nullable restore
#line 55 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
	}

#line default
#line hidden
#nullable disable
#nullable restore
#line 56 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
Write(Html.Sommario("Errori:"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\t");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d184b1c0fc4fd39cc0693a6d641b35552b7c047c11743", async() => {
                WriteLiteral("\r\n\t\t");
#nullable restore
#line 58 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.Albo));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t");
#nullable restore
#line 59 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.VariazioneRappresentanteLegale));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n\t\t");
#nullable restore
#line 61 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.IdCategoriaEnte));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t");
#nullable restore
#line 62 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
   Write(Html.HiddenFor(x => x.EnteTitolare));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 64 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.CodiceFiscaleEnte, "col-md-6", readOnly: true));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 65 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.CodiceFiscaleRappresentanteLegale, "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 68 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.DataNominaRappresentanteLegale, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 69 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Denominazione, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n");
#nullable restore
#line 71 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
         if (Model.EnteTitolare==true)
		{

#line default
#line hidden
#nullable disable
                WriteLiteral("\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 74 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.DocumentoNomina, cssClass: "col-md-12"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n");
#nullable restore
#line 76 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
		}

#line default
#line hidden
#nullable disable
                WriteLiteral("\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 78 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.ComboboxFor(x => x.IdTipologiaEnte, cssClass: "col-md-12"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<h5>Sede Legale dell\'Ente ");
#nullable restore
#line 80 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
                             Write(Model.TipoEnte);

#line default
#line hidden
#nullable disable
                WriteLiteral("</h5>\r\n\t\t<br />\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 83 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.ComboboxFor(x => x.IdProvinciaEnte, cssClass: "col-md-6", ricerca: true));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 84 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.ComboboxFor(x => x.IdComuneEnte, cssClass: "col-md-6", ricerca: true, disabled: !Model.IdProvinciaEnte.HasValue));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 87 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Via, cssClass: "col-md-10"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 88 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Civico, cssClass: "col-md-2"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 91 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.CAP, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 92 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Telefono, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 95 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Email, cssClass: "col-md-6"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t\t");
#nullable restore
#line 96 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.PEC, cssClass: "col-md-6", label: Model.EnteTitolare==true? "PEC*": "PEC"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"row\">\r\n\t\t\t");
#nullable restore
#line 99 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
       Write(Html.InputFor(x => x.Sito, cssClass: "col-md-12"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<a");
                BeginWriteAttribute("href", " href=\"", 3580, "\"", 3617, 1);
#nullable restore
#line 101 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
WriteAttributeValue("", 3587, Url.Action("Index","Accesso"), 3587, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" class=\"btn btn-primary col-md-3 col-sm-6\">Indietro</a>\r\n\t\t<button type=\"submit\" class=\"btn btn-primary col-md-3 col-sm-6\">Avanti</button>\r\n\t");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "action", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 57 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
AddHtmlAttributeValue("", 1914, Url.Action("Riepilogo"), 1914, 24, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 104 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n\t<script type=\"text/javascript\">\r\n\t\t$(\"#IdProvinciaEnte\").change(function (e) {\r\n\t\t\tvar jqxhr = $.post(\"");
#nullable restore
#line 108 "C:\temp\SistemaUnico\Registrazione\Views\Registrazione\Index.cshtml"
                           Write(Url.Action("GetComuni"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
				{ idProvincia: $(e.currentTarget).val() })
				.done(function (data) {
					$(""#IdComuneEnte_Wrapper"").setOptionsToSelect(data);
					if ($(""#IdProvinciaEnte"").val()) {
						$(""#IdComuneEnte_Wrapper"").removeAttr(""disabled"");
						$(""#IdComuneEnte_Wrapper"").find("".dropdown-toggle"").removeClass(""disabled"");
						$(""#IdComuneEnte_Wrapper"").find("".bootstrap-select"").removeAttr(""disabled"");
						$(""#IdComuneEnte"").removeAttr(""disabled"");

					} else {
						$(""#IdComuneEnte_Wrapper"").find("".dropdown-toggle"").addClass('disabled');
						$(""#IdComuneEnte_Wrapper"").find("".bootstrap-select"").prop('disabled', true);
						$(""#IdComuneEnte_Wrapper"").prop('disabled', true);
						$(""#IdComuneEnte"").prop('disabled', true);
					}
				})
				.fail(function () {
					alert(""error"");
				});

		});

	</script>

");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Registrazione> Html { get; private set; }
    }
}
#pragma warning restore 1591