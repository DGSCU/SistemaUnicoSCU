#pragma checksum "C:\temp\SistemaUnico\Registrazione\Views\Shared\_LayoutStampa.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fc52c9ef0c888f3dbc1e24b367d48abd3361f8d2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__LayoutStampa), @"mvc.1.0.view", @"/Views/Shared/_LayoutStampa.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc52c9ef0c888f3dbc1e24b367d48abd3361f8d2", @"/Views/Shared/_LayoutStampa.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"08f4fa5a7f4165ac15f3eaad47054f25f75946a0", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__LayoutStampa : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<style>

	.body {
		padding: 2em;
	}


	.logo {
		height: 80px;
		margin-bottom: 20px;
	}

	.hidePrint {
		display: none;
	}

	.panel-body ul {
		margin: 0 0 0 10px;
	}

	.text-center {
		text-align: center;
	}

	.panel {
		border: 1px solid #ddd;
		border-radius: 4px;
		padding: 10px;
		margin-top: 15px;
		margin-bottom: 15px;
	}

	ul {
		font-size: 12px;
		margin-left: 20px;
		page-break-inside: avoid;
	}

	li {
		text-align: justify;
	}

	p {
		text-align: justify;
		font-size: 12px;
		margin: 0 0 10px;
		page-break-inside: avoid;
	}


	h3 {
		font-size: 15px;
		font-family: inherit;
		line-height: 1.1;
		color: inherit;
		display: block;
		font-size: 1.17em;
		font-weight: bold;
		text-align: center;
	}
</style>


	<div class=""container body-content"">
		");
#nullable restore
#line 66 "C:\temp\SistemaUnico\Registrazione\Views\Shared\_LayoutStampa.cshtml"
   Write(RenderBody());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\t</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
