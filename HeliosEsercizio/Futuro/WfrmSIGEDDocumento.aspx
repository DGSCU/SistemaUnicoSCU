<%@ Page Language="vb" Title="Recupero Documento" AutoEventWireup="false" CodeBehind="WfrmSIGEDDocumento.aspx.vb" Inherits="Futuro.WfrmSIGEDDocumento" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Recupero Documento</title>
		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</head>
	<body>
    <form id="form1" method="post" runat="server">
     <div class="wrapper" style="width:100%">
       <div class="headers" >
        <h2>
            <asp:label id="lblAllegati" runat="server" Text="Elenco Allegati"></asp:label>
        </h2>
       </div>
        <div class= "row" >
            <div class="colOggetti" style="width:60%">
	        <asp:label id="lblTestoUP"  runat="server" CssClass="msgInfo" Text="Scaricare il file  cliccando sul pulsante " ></asp:label>
            </div>
            <div class="collable" style="width:40%">
	        <asp:hyperlink id="cmdScarica" runat="server" ImageUrl="~/Images/salva_small.png" 
                    Target="_blank" Visible="False"></asp:hyperlink>
            </div>
        </div>
          <div class= "row" >
            <div class="colOggetti" style="width:70%">
	        <asp:label id="lblNomeFile"  runat="server" CssClass="label" ></asp:label>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
