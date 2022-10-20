<%@ Page Title="Elenco documenti" Language="vb" AutoEventWireup="false" CodeBehind="WfrmSIGEDElencoDocumenti.aspx.vb" Inherits="Futuro.WfrmSIGEDElencoDocumenti" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Elenco Documenti</title>
		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</head>
	<body>
	<form id="Form1" method="post" runat="server">
    <fieldset class="fieldsetrasparente">
    <div class="wrapper" style="width:100%">
       <div class="headers" >
        <h2>
            <asp:label id="lblFascicolo" runat="server" Text="Elenco Documenti"></asp:label>
        </h2>
       </div>

     <div class= "rowGroup" style="height:auto">
            <div class="RigaVuota" >       &nbsp;       </div>
	        <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore" ></asp:label>
            <asp:label id="LblOperazioneOk" runat="server" CssClass="msgConferma" ></asp:label>
            <div class="RigaVuota" >       &nbsp;       </div>
   </div>
        <div class="row" >
            <div class="collable" >
                <asp:label id="LblNumProtocollo" AssociatedControlID="TxtNumProtocollo" runat="server" Visible="true" Text="N° Protocollo: "/>
            </div>

            <div class="colOggetti" >
                <asp:textbox id="TxtNumProtocollo" runat="server" Visible="true" Enabled="True"></asp:textbox>
            </div>
             <div class="collableRidotto">
            <asp:label id="LblAnno" AssociatedControlID="TxtAnno" runat="server" Visible="true" Text="Anno: "/>
            </div>
            <div class="colOggetti" >
                <asp:textbox id="TxtAnno" runat="server" Visible="true" MaxLength="4" ReadOnly="False"></asp:textbox>
            </div>
            </div>
              <div class="RigaPulsanti">
                <asp:Button ID="cmdAggiungi" CssClass="Pulsante" Visible="true" runat="server"  Text="Aggiungi"  />
                 <asp:Button ID="cmdIndietro" CssClass="Pulsante"  Visible="true" runat="server"  Text="Chiudi"  />&nbsp;
              </div>
 <div class= "row" >

   <h3>
     <asp:label  id="lblRisultatoRicerca" Visible="false" AssociatedControlID="dgRisultatoRicerca" Font-Bold="true"  runat="server" Text="Risultato ricerca" />
  </h3>
  
  <asp:datagrid id="dgRisultatoRicerca" runat="server"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" ShowFooter="false" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
            <asp:TemplateColumn HeaderText="Azione" > 
            <ItemTemplate>
            <asp:ImageButton ID="ImageButton1" CommandName="Select1" ToolTip="Seleziona Documento" AlternateText="Seleziona Documento" runat="server" ImageURL="images/valida_small.png" CausesValidation="false">
            </asp:ImageButton>
            </ItemTemplate>
            </asp:TemplateColumn>
		    <asp:BoundColumn Visible="False" DataField="Codice Documento" HeaderText="Codice Documento">
                <HeaderStyle  ></HeaderStyle>
			    <ItemStyle Width="5%"  ></ItemStyle>
            </asp:BoundColumn>
		    <asp:BoundColumn DataField="Data Protocollo" HeaderText="Data Protocollo" >
                <HeaderStyle></HeaderStyle>
			    <ItemStyle Width="10%"  ></ItemStyle>
            </asp:BoundColumn>
		    <asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione">
                <HeaderStyle></HeaderStyle>
				<ItemStyle Width="50%"  ></ItemStyle>
            </asp:BoundColumn>
		    <asp:BoundColumn DataField="Numero" HeaderText="Numero" >
                <HeaderStyle ></HeaderStyle>
				<ItemStyle Width="15%"  ></ItemStyle>
            </asp:BoundColumn>
		    <asp:BoundColumn DataField="Tipo Documento" HeaderText="Tipo Documento">
                <HeaderStyle></HeaderStyle>
			    <ItemStyle Width="20%"  ></ItemStyle>
            </asp:BoundColumn>
	    </Columns>
        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
    </asp:datagrid>

    </div>
    
    </div>
    </fieldset>
		</form>
	</body>
</html>