<%@ Page Title="InfoGenerazioneElencoSedi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmInfoPresentazioneAccreditamento.aspx.vb" Inherits="Futuro.WfrmInfoPresentazioneAccreditamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="refresh" content="2"/>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>INFO PRESENTAZIONE ACCREDITAMENTO/ADEGUAMENTO</legend>
        <div class="wrapper1">
		    <div class="row">
			    <div >
			    </div>
		    </div >
		    <div class="row" style="height:230px">
			    <div class="collable" style="width:50%">
				    <img id="imgAttesa" alt="Attesa" src="images/wait2.gif" width="220" height="220" />
			    </div>
			    <div  class="colOggetti" style="width:50%; height: 230px;">
                <h2>
				    <asp:label id="LblInfo" runat="server"> Elaborazione degli allegati in corso. Si prega di attendere il completamento dell&#39;operazione.</asp:label>
                    </h2>   
                    </div>
		    </div >
		    <div class="row">
			    <div class="collable" style="width:50%">
                <h2>
                <asp:label id="LblNumProgPresentati" runat="server">Totale Allegati da generare:</asp:label>
				    <asp:label id="LblTotPre" runat="server"></asp:label></h2>
			    </div>
		    </div >
		    <div class="row">
                   
			    <div class="collable" style="width:50%"> <h2>
                <asp:label id="LblNumProgRimasti" runat="server">Allegati mancanti:</asp:label>
				    <asp:label id="LblTotElab" runat="server"></asp:label></h2>
			    </div>
		    </div>
		    <div class="RigaPulsanti">
			    <asp:button id="imgChiudi" CssClass="Pulsante" text="Chiudi" Runat="server" />
		    </div>
	    </div>
    </fieldset>

</asp:Content>
