<%@ Page Title="Gestione Sedi Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebGestioneSediProgetto.aspx.vb" Inherits="Futuro.WebGestioneSediProgetto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
<script type="text/javascript">
  /* <![CDATA[ */
    var idErrore = CostruisciId("lblmessaggio");
    //function VisualizzaSanzione(IDEnteSedeAttuazione, IdEnte) {
   //     window.open("WfrmSedeSanzionata.aspx?IDEnteSedeAttuazione=" + IDEnteSedeAttuazione + "&IdEnte=" + IdEnte, "SedeSanzionata", "width=950, height=600, toolbar=no, location=no, menubar=no, scrollbars=yes");
   // };
    function VerificaNumeroIntero(idCampo, descrizioneCampo) {

        VerificaNumeroIntero(CostruisciId(idCampo), descrizioneCampo, idErrore);
         
    };
      
      /* ]]> */
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 &nbsp;<fieldset class="ContornoPagina ">
<legend >Gestione Sedi Progetto</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblIntProgetto" runat="server"  Text="Ricerca Sedi Per Il progetto: '"></asp:Label>
      <asp:label  id="lblProgetto" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      <asp:label  id="lblApiceFineProgetto" style="vertical-align:middle" Text="'" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
         <asp:HiddenField ID="txtModifica" runat="server" />
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
    
    <div class="row" >
    
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblSedeFisica" CssClass="label" AssociatedControlID="txtSedefisica"  runat="server" Text="Codice Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtSedefisica" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>
        </div>
          <div class="collable" style="width:15%">
           <asp:Label ID="Label2" CssClass="label" AssociatedControlID="txtCodiceSede" runat="server" Text="Codice Sede"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox  ID="txtCodiceSede" CssClass="textbox"  onchange="javascript: VerificaNumeroIntero('txtCodiceSede','Codice Sede');" runat="server"></asp:TextBox>       
       </div>
    </div>


    <div class="row" >  
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblSedeAttuazione" CssClass="label" AssociatedControlID="txtSedeAttuaz"  runat="server" Text="Sede Attuazione"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtSedeAttuaz" CssClass="textbox" runat="server"></asp:TextBox>
        </div>
            <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblRegione" CssClass="label" AssociatedControlID="txtRegione"  runat="server" Text="Regione"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>
        </div>
 
    </div>
    <div class="row" >
    
         <div class="collable" style="width:15%">
           <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="txtprovincia" runat="server" Text="Provincia"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox  ID="txtprovincia" CssClass="textbox"  runat="server"></asp:TextBox>
       </div>
            <div class="colOggetti" style="width:15%">
            <asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtComune"  runat="server" Text="Comune"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row" > 

        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblPresenzaSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione"  runat="server" Text="Presenza Sanzione"></asp:Label>
        &nbsp;</div>
        <div class="colOggetti" style="width:35%" >
           <asp:dropdownlist id="ddlSegnalazioneSanzione" runat="server" CssClass="ddlClass" >
				<asp:ListItem Selected="True">Tutti</asp:ListItem>
				<asp:ListItem Value="0">No</asp:ListItem>
				<asp:ListItem Value="1">Si</asp:ListItem>
			</asp:dropdownlist>
        &nbsp;</div>
         <div class="collable" style="width:50%">
           <asp:CheckBox  ID="chkSediProgetto" runat="server" Text="Visualizza solo sedi selezionate" />
       </div>

    </div>
     <div class="row" > 
     <div class="colOggetti" style="width:15%">
            &nbsp;</div>
      <div class="colOggetti" style="width:35%" >
       &nbsp;
      </div>
     
        <div class="colOggetti" style="width:35%" >
            <asp:CheckBox  ID="chkSediNonIscritte" runat="server" 
                Text="Visualizza solo sedi non iscritte" Visible="false" />
        </div>

    </div>
    <div class="RigaPulsanti" > 
      
       <asp:Button id="cmdRicerca" CssClass="Pulsante"  runat="server" Text="Ricerca" ></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>&nbsp;
</div>
<div class="RigaPulsanti">
 <asp:Button id="imgAggiungi" CssClass="Pulsante"  runat="server" 
        Text="Aggiungi sede UE non iscritta" Visible="false"></asp:Button>
</div>
</div>
<div class="RigaVuota">&nbsp;</div>

  <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca Sedi Progetto"   
            runat="server" Width="100%" ToolTip="Sedi Progetto" 
                   AllowPaging="True"  CssClass="table" AllowSorting="True" 
                   AutoGenerateColumns="False" UseAccessibleHeader="True">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
     <asp:BoundColumn DataField="CodiceRegione" HeaderText="Codice Ente"></asp:BoundColumn>
		<asp:BoundColumn DataField="sedeFisica" HeaderText="Sede">
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo">
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Comune" HeaderText="Comune ">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="telefono" HeaderText="telefono">
		</asp:BoundColumn>
		<asp:BoundColumn DataField="sedeattuazione" HeaderText="Sede Attuazione" Visible="False" >
		</asp:BoundColumn>
		<asp:BoundColumn DataField="statoentesede" HeaderText="Stato"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="nProgAtt" HeaderText="N&#176; Progetti Attivi">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idEntesede" HeaderText="idSede"></asp:BoundColumn>
		<asp:BoundColumn DataField="idEntesedeattuazione" HeaderText="Codice Sede"></asp:BoundColumn>
        <asp:BoundColumn DataField="totNumeroPosti"  HeaderText="N&#186; Volontari">
        <ItemStyle Width="5%" />
        </asp:BoundColumn>
		<asp:BoundColumn Visible="False" HeaderText="NVol"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idatt" HeaderText="idAttivitaES"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="Attiva" HeaderText="Attiva"></asp:BoundColumn>
		<asp:BoundColumn DataField="DataCreazioneRecord" HeaderText="Data Inserimento" DataFormatString="{0:d}">
        <ItemStyle Width="7%" />
        </asp:BoundColumn>
		<asp:BoundColumn DataField="PresenzaSanzione" HeaderText="Presenza Sanzione">
        <ItemStyle Width="5%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="verifica" HeaderText="Presenza Verifica">
        <ItemStyle Width="5%" />
		</asp:BoundColumn>
        <asp:BoundColumn Visible="false" DataField="StatoEnte" HeaderText="Stato Ente">
		</asp:BoundColumn>
        <asp:TemplateColumn  HeaderText="Aggiungi / Modifica"> 
           <ItemTemplate >
				<asp:ImageButton  DataField="prova" ID="AggiungiModifica" style="cursor:pointer;"  CommandName="AggiungiModificaSede" AlternateText="Aggiungi Sede" ToolTip='Aggiungi Sede' runat="server" ImageUrl="images/valida_small.png" Visible='<%# IIF(Eval("prova").ToString().Equals("NO"), False, True) %>' ></asp:ImageButton>      
          </ItemTemplate>
          <ItemStyle Width="5%" />

        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Rimuovi"> 
            <ItemTemplate >
			    <asp:ImageButton ID="ImageButton2"  style="cursor:pointer;" CommandName="Rimuovi" AlternateText="Rimuovi Sede" ToolTip='Rimuovi Sede' runat="server" ImageURL="images/canc_small.png" CausesValidation="false" Visible='<%# IIF(Eval("prova").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>      
            </ItemTemplate>
            <ItemStyle Width="5%" />
        </asp:TemplateColumn>
       
	</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
</asp:datagrid>
</fieldset>
 
</asp:Content>
