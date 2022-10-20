<%@ Page Title="Dettagli Progetti Volontari" Language="vb" AutoEventWireup="false" CodeBehind="WfrmAttivitaVolontari.aspx.vb" Inherits="Futuro.WfrmAttivitaVolontari" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Dettagli Progetti Volontari </title>
    		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
</head>
<body>
	<form id="Form1" method="post" runat="server" >
    <fieldset class="ContornoPagina">
    <legend> Dettagli volontario</legend>
    <div class="wrapper" style="width:100%;border:0px">
      	 <div class="row" style="height:auto">
		    <asp:label id="lblError" runat="server" CssClass="msgErrore"></asp:label>
		</div>
    <div class="RigaVuota">&nbsp;
    </div>
    <div class="row" style="height:auto">
        <div class="colHyperLink" style="width:100%;"  >
            <asp:LinkButton  ID="Imagebutton1"   style="cursor:pointer" Text="Cronologia Dettaglio Anagrafica" ToolTip="Cronologia Anagrafica"  runat="server" />
            </div>
        <div class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="imgCronoDocu" style="cursor:pointer" Text="Cronologia Documenti" ToolTip="Cronologia Documenti"  runat="server" />
        </div>
        <div class="colHyperLink" style="width:100%">
        
        <asp:LinkButton ID="imgCronoRiattivazione" style="cursor:pointer" Text="Cronologia Riattivazione" ToolTip="Cronologia Riattivazione" Visible="false"  runat="server" />
        <asp:LinkButton ID="imgCronoRimodulazione" style="cursor:pointer" Text="Cronologia Rimodulazione" ToolTip="Cronologia Rimodulazione" Visible="false"  runat="server" />
        </div>

    </div>
        <div class="RigaVuota">&nbsp;
    </div>
    <div class="rowGroup" id="DivDatiVolontario" runat="server">
        <div class="row" style="height:30px">
    
    <div class="collable" style="width:15%" >
        <asp:Label ID="lblCodEnte" AssociatedControlID="txtCodente"  runat="server" Text="Ente"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label ID="txtCodente" CssClass="labelDati" Text="&nbsp;"  Enabled="false"   runat="server"></asp:Label>
    </div>
    <div class="collable" style="width:15%">
        <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtDenominazione" runat="server" Text="Denominazione"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
            <asp:Label ID="txtDenominazione" Text="&nbsp;" CssClass="labelDati" runat="server"></asp:Label>
    </div>
    </div>
       <div class="row" style="height:30px">
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblNominativo" AssociatedControlID="txtNominativo"  runat="server" Text="Nominativo"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtNominativo" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="txtComuneNascita" runat="server" Text="Comune di Nascita"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtComuneNascita" Text="&nbsp;" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
    
    <div class="collable" style="width:15%" >
        <asp:Label ID="LblDataNascita" AssociatedControlID="txtDataNascita"  runat="server" Text="Data Nascita"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label ID="txtDataNascita" CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
    </div>
    <div class="collable" style="width:15%">
        <asp:Label ID="LblCf" CssClass="label" AssociatedControlID="txtCF" runat="server" Text="Codice Fiscale"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
            <asp:Label ID="txtCF" CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
    </div>
    </div>
       <div class="row" style="height:30px">
           <div class="collable" style="width:15%" >
               <asp:Label ID="LblIndirizzo" AssociatedControlID="txtIndirizzo"  runat="server" Text="Indirizzo"></asp:Label>
           </div>
           <div class="colOggetti" style="width:85%">
               <asp:Label ID="txtIndirizzo" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
           </div>
       </div>
       <div class="row" style="height:30px">
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblUPresentazione" AssociatedControlID="txtUPresentazione"  runat="server" Text="User Presentante"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtUPresentazione" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LblDPresentazione" CssClass="label" AssociatedControlID="txtDPresentazione" runat="server" Text="Data Presentazione"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtDPresentazione"  CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblUConferma" AssociatedControlID="txtUConferma"  runat="server" Text="User Confermante"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtUConferma" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LblDConferma" CssClass="label" AssociatedControlID="txtDConferma" runat="server" Text="Data Conferma"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtDConferma"  CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblUChiusura" AssociatedControlID="txtUChiusura"  runat="server" Text="User Chiusura"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtUChiusura" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LblDChiusura" CssClass="label" AssociatedControlID="txtDChiusura" runat="server" Text="Data Esecuzione  Chiusura"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtDChiusura"  CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
       <div class="collable" style="width:15%" >
           <asp:Label ID="LabelStato" AssociatedControlID="lblStato"  runat="server" Text="Stato"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblStato" CssClass="labelDati" Text="&nbsp;"  Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LabelCausale" CssClass="label" AssociatedControlID="lblCausale" runat="server" Text="Causale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="lblCausale"  CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblSubentrato" AssociatedControlID="txtSubentrato"  runat="server" Text="Subentrato&nbsp;a"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtSubentrato" CssClass="labelDati"  Enabled="false" Text="&nbsp;"  runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LblSostituito" CssClass="label" AssociatedControlID="txtSostituito" runat="server" Text="Sostituito&nbsp;da"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtSostituito" Text="&nbsp;" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblUserAttestato" AssociatedControlID="txtUserAttestato"  runat="server" Text="User Attestato"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtUserAttestato" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LblDataAttestato" CssClass="label" AssociatedControlID="txtDataAttestato" runat="server" Text="Data Attestato"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtDataAttestato"  CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblStatoAttestato" AssociatedControlID="txtStatoAttestato"  runat="server" Text="Stato Attestato"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtStatoAttestato" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" style="height:30px">
       <div class="collable" style="width:15%" >
           <asp:Label ID="LabelNomeBando" AssociatedControlID="lblNomeBando"  runat="server" Text="Nome Bando"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblNomeBando" CssClass="labelDati" Text="&nbsp;"  Enabled="false"   runat="server"></asp:Label>
       </div>
         <div class="collable" style="width:15%" >
           <asp:Label ID="LabelNVol" CssClass="label" AssociatedControlID="lblNVol" runat="server" Text="N&deg; Volontari"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="lblNVol"  CssClass="labelDati" Text="&nbsp;" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row"  >
       <div class="collable" style="width:15%" >
           <asp:Label ID="LabelGazzetta" AssociatedControlID="lblGazzetta"  runat="server" Text="Gazzetta Ufficiale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">
           <asp:Label ID="lblGazzetta" CssClass="labelDati" Text="&nbsp;" Enabled="false"   runat="server"></asp:Label>
       </div>
       </div>
       <div class="row"  >
           <div class="collable" style="width:15%" >
               <asp:Label ID="LabelSedeSecondaria" AssociatedControlID="LblSedeSecondaria"  runat="server" Text="Sede Secondaria"></asp:Label>
           </div>
           <div class="colOggetti" style="width:85%">
               <asp:Label ID="LblSedeSecondaria" CssClass="labelDati" Text="&nbsp;" Enabled="false" runat="server"></asp:Label>
           </div>
       </div>
        <h3>
<asp:label id="lblRisultato"  runat="server" AssociatedControlID="dtgRisultatoRicerca" CssClass="msgInfo"  Text="Elenco Sedi di Servizi"></asp:label>
</h3>
    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Volontari" 
		    AllowPaging="false" CellPadding="2"  CssClass="table"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn DataField="titolo" HeaderText="Progetto">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataInizioAssegnazione" HeaderText="Data Inizio Progetto">
				<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataFineAssegnazione" HeaderText="Data Fine Progetto">
				<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="StatoAttivit&#224;" HeaderText="Stato Progetto">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Sede" HeaderText="Sede">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Sedeattuazione" HeaderText="Sede Attuazione">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="comuneSede" HeaderText="Comune Sede">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataInizioAttivit&#224;Entit&#224;" HeaderText="Data Inizio Servizio">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataFineAttivit&#224;Entit&#224;" HeaderText="Data Fine Servizio">
					<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Usernamericollocamento" HeaderText="User Ricollocamento"></asp:BoundColumn>
        </Columns>				
    </asp:datagrid>


     <h3>
        <asp:label id="lblStoricoAssegnazioni"  runat="server" AssociatedControlID="DtgStoricoAssegnazione" CssClass="msgInfo"  Text="Storico Graduatoria"></asp:label>
    </h3>
    <asp:datagrid id="DtgStoricoAssegnazione" runat="server" Width="100%" ToolTip="Elenco Volontari" 
		    AllowPaging="true" CellPadding="2"  CssClass="table" PageSize="5" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn DataField="CodiceRegione" HeaderText="Codice Ente">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DenominazioneEnte" HeaderText="Nome Ente">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Titolo" HeaderText="Titolo">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="ComuneSede" HeaderText="Comune Sede">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="TMPIdSedeAttuazioneOriginale" HeaderText="Codice Sede">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="UserNameAssegnazioneAltraSede" HeaderText="User Assegnazione">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataAssegnazioneAltraSede" HeaderText="Data Assegnazione">
						<HeaderStyle ></HeaderStyle>
				<ItemStyle ></ItemStyle>
			</asp:BoundColumn>
		</Columns>
			<PagerStyle NextPageText="&gt;&gt;"	PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>					
    </asp:datagrid>
    </div>
    <div class="RigaVuota">&nbsp;</div>
      <div class="RigaPulsanti">
        <asp:Button ID="cmdChiudi"  runat="server" OnClientClick="javascript: window.close()" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
    </div> 
    </div>
    </fieldset>
    </form>
</body>
</html>
