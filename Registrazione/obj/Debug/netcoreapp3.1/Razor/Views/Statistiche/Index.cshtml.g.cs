#pragma checksum "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "89d2ed5d273ac9a712123ed581dd1d9ff39308bb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Statistiche_Index), @"mvc.1.0.view", @"/Views/Statistiche/Index.cshtml")]
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
#line 1 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
using RegistrazioneSistemaUnico.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
using RegistrazioneSistemaUnico.Models.Forms;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"89d2ed5d273ac9a712123ed581dd1d9ff39308bb", @"/Views/Statistiche/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"08f4fa5a7f4165ac15f3eaad47054f25f75946a0", @"/Views/_ViewImports.cshtml")]
    public class Views_Statistiche_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<StatisticheForm>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("frmStatistiche"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onsubmit", new global::Microsoft.AspNetCore.Html.HtmlString("return OnSubmitCheck();"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Chartjs/dist/Chart.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
  
	ViewData["Title"] = "Home Page";

	string errorMessage = TempData["Error"]?.ToString();
	Statistiche statistiche = ViewData["Statistiche"] as Statistiche;

#line default
#line hidden
#nullable disable
            WriteLiteral("<h3>Statistiche</h3>\r\n<br />\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "89d2ed5d273ac9a712123ed581dd1d9ff39308bb5479", async() => {
                WriteLiteral("\r\n\t<input type=\"hidden\" id=\"hdCheck\" />\r\n\t<div class=\"row\">\r\n\t\t<div class=\"col-4\">\r\n\t\t\t");
#nullable restore
#line 16 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
       Write(Html.InputFor(x => x.DataDa));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"col-2\">\r\n\t\t\t");
#nullable restore
#line 19 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
       Write(Html.InputFor(x => x.OrarioDa, tooltip: "hh:mm"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"col-4\">\r\n\t\t\t");
#nullable restore
#line 22 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
       Write(Html.InputFor(x => x.DataA));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"col-2\">\r\n\t\t\t");
#nullable restore
#line 25 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
       Write(Html.InputFor(x => x.OrarioA, tooltip: "hh:mm"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\t\t</div>\r\n\t\t<div class=\"col-4\">\r\n\t\t\t<button type=\"submit\" onclick=\"SubmitOk();\" class=\"btn btn-primary col-12\">Filtra</button>\r\n\r\n\t\t</div>\r\n\t</div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "action", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 12 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
AddHtmlAttributeValue("", 346, Url.Action("Index","Statistiche"), 346, 34, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

<hr />

<h4>Statistiche registrazioni</h4>
<br />
<div class=""row"">
	<div class=""col-6"">
		<canvas id=""diagrammaTorta"" height=""300""></canvas>
	</div>
	<div class=""col-6"">
		<canvas id=""diagrammaTorta2"" height=""300""></canvas>
	</div>
</div>
<br />
<p>Registrazioni effettuate: <strong>");
#nullable restore
#line 47 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                                Write(statistiche.TotaleRegistrazioni);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong></p>\r\n\r\n<div class=\"row\">\r\n\t<div class=\"col-12\">\r\n\t\t<canvas id=\"diagrammaAndamento\" height=\"400\"></canvas>\r\n\t</div>\r\n\r\n</div>\r\n<hr />\r\n<h3>Statistiche accessi</h3>\r\n<p>Accessi effettuati: <strong>");
#nullable restore
#line 57 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                          Write(statistiche.Accessi);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong></p>\r\n<p>Utenti distinti: <strong>");
#nullable restore
#line 58 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.UtentiDistinti);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong></p>\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n\t");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "89d2ed5d273ac9a712123ed581dd1d9ff39308bb10378", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"

	<script type=""text/javascript"">
		function OnSubmitCheck() {
			if (!$(""#hdCheck"").val()) {
				return false;
			}

		}
		function SubmitOk() {
			$(""#hdCheck"").val(""OK"");
			$(""#frmStatistiche"").submit();

		}
		$(document).ready(function () {
");
                WriteLiteral("\t\t\tvar datiTorta = {\r\n\t\t\t\tdatasets: [{\r\n\t\t\t\t\tlabel: \'Registrazioni Enti\',\r\n\t\t\t\t\tdata: [\t");
#nullable restore
#line 82 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                          Write(statistiche.EntiRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n\t\t\t\t\t\t\t");
#nullable restore
#line 83 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.EntiNonRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n\t\t\t\t\t\t\t");
#nullable restore
#line 84 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.NuoviEntiRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n\t\t\t\t\t\t\t");
#nullable restore
#line 85 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.VariazioniRappresentanteLegale);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"],
					backgroundColor: [
						'rgba(54, 235, 162, 0.2)',
						'rgba(54, 162, 235, 0.2)',
						'rgba(255, 206, 86, 0.2)',
						'rgba(255, 99, 132, 0.2)',

					],
					borderColor: [
						'rgba(54, 235, 162, 1)',
						'rgba(54, 162, 235, 1)',
						'rgba(255, 206, 86, 1)',
						'rgba(255,99,132,1)',

					],
					borderWidth: 1,

				}],
				labels: [
					'Enti Registrati',
					'Enti Non Registrati',
					'Nuovi Enti Registrati',
					'Variazioni Rapp. Leg.'
				]
			};
			var ChartTorta = $('#diagrammaTorta');
			var myPieChart = new Chart(ChartTorta, {
				type: 'pie',
				data: datiTorta,
				options: {
					responsive: true,
					maintainAspectRatio: false,
					title: {
						display: true,
						text: 'Registrazioni Totali'
					},
					legend: {
						display: true,
						position : 'right'
					}
				}
			});

");
                WriteLiteral("\t\t\tvar datiTorta2 = {\r\n\t\t\t\tdatasets: [\r\n\t\t\t\t{\r\n\t\t\t\t\tlabel: \'Registrazioni Enti\',\r\n\t\t\t\t\tdata: [\t");
#nullable restore
#line 133 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                          Write(statistiche.EntiTitolariPubbliciRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n\t\t\t\t\t\t\t");
#nullable restore
#line 134 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.EntiTitolariPrivatiRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n\t\t\t\t\t\t\t");
#nullable restore
#line 135 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.EntiAccoglienzaPubbliciRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(",\r\n\t\t\t\t\t\t\t");
#nullable restore
#line 136 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                       Write(statistiche.EntiAccoglienzaPrivatiRegistrati);

#line default
#line hidden
#nullable disable
                WriteLiteral(@",
					],
					backgroundColor: [
						'rgba(54, 162, 235, 0.2)',
						'rgba(54, 162, 235, 0.4)',
						'rgba(255, 206, 86, 0.2)',
						'rgba(255, 206, 86, 0.4)'

					],
					borderColor: [
						'rgba(54, 162, 235, 1)',
						'rgba(54, 162, 235, 1)',
						'rgba(255, 206, 86, 1)',
						'rgba(255, 206, 86, 1)'

					],
					borderWidth: 1,

				}],
				labels: [
					'Enti Titolari Pubblici',
					'Enti Titolari Privati',
					'Enti Di Accoglienza Pubblici',
					'Enti Di Accoglienza Privati',
				]
			};
			var ChartTorta2 = $('#diagrammaTorta2');
			var myPieChart = new Chart(ChartTorta2, {
				type: 'pie',
				data: datiTorta2,
				options: {
					responsive: true,
					maintainAspectRatio: false,
					title: {
						display: true,
						text: 'Tipolgia Enti Registrati'
					},
					legend: {
						display: true,
						position: 'right'
					}
				}
			});


		/*Grafico Lineare*/




			var ChartAndamento = $('#diagrammaAndamento');

			var myLineCha");
                WriteLiteral("rt = new Chart(ChartAndamento, {\r\n\t\t\t\ttype: \'line\',\r\n\t\t\t\tdata: {\r\n\t\t\t\t\tlabels: [");
#nullable restore
#line 191 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                        Write(Html.Raw(ViewData["AndamentoX"]));

#line default
#line hidden
#nullable disable
                WriteLiteral("],\r\n\t\t\t\t\tdatasets: [{\r\n\t\t\t\t\t\tlabel: \'Registrazioni\',\r\n\r\n\t\t\t\t\t\tdata: [");
#nullable restore
#line 195 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                          Write(Html.Raw(ViewData["Andamento"]));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"],
						backgroundColor: [
							'rgba(255, 99, 132, 0.2)'

						],
						borderColor: [
							'rgba(54, 162, 235, 1)'
						],
						fill: false,
					}]
				},
				options: {
					responsive: true,
					maintainAspectRatio: false,
					title: {
						display: true,
						text: 'Andamento Registrazioni'
					},
					tooltips: {
						mode: 'index',
						intersect: false,
					},
					hover: {
						mode: 'nearest',
						intersect: true
					},
					scales: {
						xAxes: [{
							display: true,
							scaleLabel: {
								display: true,
								labelString: '");
#nullable restore
#line 226 "C:\temp\SistemaUnico\Registrazione\Views\Statistiche\Index.cshtml"
                                         Write(ViewData["AndamentoXLabel"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"'
							}
						}],
						yAxes: [{

							display: true,
							scaleLabel: {
								display: true,
								labelString: 'N° Registrazioni'
							},
							ticks: {
								beginAtZero: true
							}

						}]
					}
				}
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<StatisticheForm> Html { get; private set; }
    }
}
#pragma warning restore 1591
